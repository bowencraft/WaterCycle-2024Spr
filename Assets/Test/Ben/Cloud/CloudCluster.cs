using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCluster : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody self;
    [SerializeField] private CloudElement elementPrefab;

    [Header("Config")]
    [SerializeField] [Min(0)] public int elementCount;
    [SerializeField] private float horizontalSpreadFactor;
    [SerializeField] private float verticalSpreadFactor;

    [HideInInspector] public List<CloudElement> elements = new List<CloudElement>();

    [Header("Grow Settings")]
    [SerializeField] private float cloudGrowTick;
    [SerializeField] private int cloudSizeMax;

    private bool active = true;
    private float cloudGrowTimer = 0f;

    private void Start()
    {
        for (int i = 0; i < elementCount; i++)
        {
            Vector3 elementOffset = Random.insideUnitSphere;
            elementOffset.x *= horizontalSpreadFactor * elementCount;
            elementOffset.z *= horizontalSpreadFactor * elementCount;
            elementOffset.y = Mathf.Abs(elementOffset.y) * verticalSpreadFactor * elementCount;

            CloudElement newElement = Instantiate(elementPrefab, transform.position, Quaternion.identity);
            newElement.Setup(null, self, elementOffset);
            elements.Add(newElement);
        }
    }

    private void Update()
    {
        if (active && elementCount < cloudSizeMax)
        {
            cloudGrowTimer += Time.deltaTime;

            if (cloudGrowTimer >= cloudGrowTick)
            {
                cloudGrowTimer = 0f;
                IncreaseSize(1);
            }
        }
    }

    private void IncreaseSize(int amount)
    {
        elementCount += amount;

        for (int i = 0; i < amount; i++)
        {
            Vector3 elementOffset = Random.insideUnitSphere;
            elementOffset.x *= horizontalSpreadFactor * elementCount;
            elementOffset.z *= horizontalSpreadFactor * elementCount;
            elementOffset.y = Mathf.Abs(elementOffset.y) * verticalSpreadFactor * elementCount;

            CloudElement newElement = Instantiate(elementPrefab, transform.position, Quaternion.identity);
            newElement.Setup(null, self, elementOffset);
            elements.Add(newElement);
        }

        UpdateCloud();
    }

    private void UpdateCloud()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cloud"))
        {
            active = false;
            MergeWithCloud(other.GetComponent<CloudController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cloud"))
            active = true;
    }

    private void MergeWithCloud(CloudController cloudController)
    {
        CloudAppearance cloudAppearance = cloudController.appearance;

        cloudController.rb.position = self.position;
        cloudAppearance.elementCount += elementCount;

        cloudAppearance.UpdateCloud();

        foreach (CloudElement element in elements)
        {
            cloudAppearance.elements.Add(element);
            element.Setup(cloudAppearance, cloudController.rb);
        }

        elements.Clear();
        elementCount = 0;
    }
}
