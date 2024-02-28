using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAppearance : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public CloudController controller;
    [SerializeField] private CloudElement elementPrefab;

    [Header("Config")]
    [SerializeField] [Min(0)] public int elementCount;
    [SerializeField] private float horizontalSpreadFactor;
    [SerializeField] private float verticalSpreadFactor;

    [HideInInspector] public List<CloudElement> elements = new List<CloudElement>();

    [Header("Rain Settings")]
    [SerializeField] private float cloudShrinkTick;

    private float cloudShrinkTimer = 0f;

    private void Start()
    {
        for (int i = 0; i < elementCount; i++)
        {
            Vector3 elementOffset = Random.insideUnitSphere;
            elementOffset.x *= horizontalSpreadFactor * elementCount;
            elementOffset.z *= horizontalSpreadFactor * elementCount;
            elementOffset.y = Mathf.Abs(elementOffset.y) * verticalSpreadFactor * elementCount;

            CloudElement newElement = Instantiate(elementPrefab, transform.position, Quaternion.identity);
            newElement.Setup(this, controller.rb, elementOffset);
            elements.Add(newElement);
        }
    }

    private void Update()
    {
        UpdateRain();
    }

    public void RemoveElement(CloudElement element)
    {
        elements.Remove(element);
        elementCount--;

        element.Death();

        UpdateCloud();

        if (elementCount <= 0)
            CloudController.TransitionToRaindrop(element.self.position, element.self.velocity);
    }

    private void RemoveFurthestElement()
    {
        CloudElement furthestElement = elements[0];
        float furthestDistance = Vector3.Distance(transform.position, furthestElement.transform.position);

        foreach (CloudElement element in elements)
        {
            float distance = Vector3.Distance(transform.position, element.transform.position);

            if (distance > furthestDistance)
            {
                furthestElement = element;
                furthestDistance = distance;
            }
        }

        RemoveElement(furthestElement);
    }

    public void UpdateCloud()
    {
        foreach (CloudElement element in elements)
        {
            Vector3 elementOffset = Random.insideUnitSphere;
            elementOffset.x *= horizontalSpreadFactor * elementCount;
            elementOffset.z *= horizontalSpreadFactor * elementCount;
            elementOffset.y = Mathf.Abs(elementOffset.y) * verticalSpreadFactor * elementCount;

            element.SetOffset(elementOffset);
        }
    }

    private void UpdateRain()
    {
        if (Input.GetKeyDown(KeyCode.Space)) StartRaining();
        if (Input.GetKeyUp(KeyCode.Space)) StopRaining();

        if (Input.GetKey(KeyCode.Space))
        {
            cloudShrinkTimer += Time.deltaTime;

            if (cloudShrinkTimer >= cloudShrinkTick)
            {
                RemoveFurthestElement();
                cloudShrinkTimer = 0f;
            }
        }
    }

    public void StartRaining()
    {
        foreach (CloudElement element in elements)
            element.StartRaining();
    }

    public void StopRaining()
    {
        foreach (CloudElement element in elements)
            element.StopRaining();
    }
}
