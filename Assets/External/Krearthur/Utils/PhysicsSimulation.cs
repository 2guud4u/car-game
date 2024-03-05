﻿/*
MIT License

Copyright (c) 2017 Sebastian Lague

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class PhysicsSimulation : MonoBehaviour
{

    public static int maxIterations = 1000;
    [SerializeField] static SimulatedBody[] simulatedBodies;

    public static Vector2 forceMinMax;
    public static float forceAngleInDegrees;
    public static bool randomizeForceAngle;

    static List<Rigidbody> generatedRigidbodies;
    static List<Collider> generatedColliders;

    [ContextMenu("Run Simulation")]
    public static void RunSimulation(GameObject[] gameObjects)
    {
        if (gameObjects == null) return;
        Transform[] transforms = gameObjects
            .Where(go => go.TryGetComponent(out MeshFilter filter)) // ignore objects that dont have a mesh comp
            .Select(go => go.GetComponent<Transform>()).ToArray();

        AutoGenerateComponents(transforms);

        // Add force to bodies
        foreach (SimulatedBody body in simulatedBodies)
        {
            if (body.isChild)
            {
                float randomForceAmount = Random.Range(forceMinMax.x, forceMinMax.y);
                float forceAngle = ((randomizeForceAngle) ? Random.Range(0, 360f) : forceAngleInDegrees) * Mathf.Deg2Rad;
                Vector3 forceDir = new Vector3(Mathf.Sin(forceAngle), 0, Mathf.Cos(forceAngle));
                body.rigidbody.AddForce(forceDir * randomForceAmount, ForceMode.Impulse);
            }
        }

        // Run simulation for maxIteration frames, or until all child rigidbodies are sleeping
        SimulationMode oldMode = Physics.simulationMode;
        Physics.simulationMode = SimulationMode.Script;
        for (int i = 0; i < maxIterations; i++)
        {
            Physics.Simulate(Time.fixedDeltaTime);
            if (simulatedBodies.All(body => body.rigidbody.IsSleeping() || !body.isChild))
            {
                break;
            }
        }
        Physics.simulationMode = oldMode;

        // Reset bodies which are not child objects of the transform to which this script is attached
        foreach (SimulatedBody body in simulatedBodies)
        {
            if (!body.isChild)
            {
                body.Reset();
            }
        }

        RemoveAutoGeneratedComponents();

    }

    // Automatically add rigidbody and box collider to object if it doesn't already have
    // and fill the generatedBodies array
    static void AutoGenerateComponents(Transform[] transforms)
    {
        generatedRigidbodies = new List<Rigidbody>();
        generatedColliders = new List<Collider>();
        simulatedBodies = new SimulatedBody[transforms.Length];

        int i = 0;
        foreach (Transform child in transforms)
        {            
            if (!child.TryGetComponent(out Rigidbody rb))
            {
                rb = child.gameObject.AddComponent<Rigidbody>();
                generatedRigidbodies.Add(rb);
            }
            if (!child.GetComponent<Collider>())
            {
                generatedColliders.Add(child.gameObject.AddComponent<BoxCollider>());
            }

            simulatedBodies[i] = new SimulatedBody(rb, true);
            i++;
        }
    }

    // Remove the components which were generated at start of simulation
    static void RemoveAutoGeneratedComponents()
    {
        foreach (Rigidbody rb in generatedRigidbodies)
        {
            DestroyImmediate(rb);
        }
        foreach (Collider c in generatedColliders)
        {
            DestroyImmediate(c);
        }
    }

    [ContextMenu("Reset")]
    public static void ResetAllBodies()
    {
        if (simulatedBodies != null)
        {
            foreach (SimulatedBody body in simulatedBodies)
            {
                body.Reset();
            }
        }
    }

    struct SimulatedBody
    {
        public readonly Rigidbody rigidbody;
        public readonly bool isChild;
        readonly Vector3 originalPosition;
        readonly Quaternion originalRotation;
        readonly Transform transform;

        public SimulatedBody(Rigidbody rigidbody, bool isChild)
        {
            this.rigidbody = rigidbody;
            this.isChild = isChild;
            transform = rigidbody.transform;
            originalPosition = rigidbody.position;
            originalRotation = rigidbody.rotation;
        }

        public void Reset()
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
}