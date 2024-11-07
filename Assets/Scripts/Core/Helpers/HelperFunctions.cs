using System.Collections.Generic;
using UnityEngine;

namespace BaseGame
{
    public static class HelperFunctions
    {
        /// <summary>
        /// Attempts to find the first GameObject with the specified tag and returns its transform.
        /// </summary>
        /// <param name="tag">The tag to search for the GameObject.</param>
        /// <param name="transform">Output parameter that holds the Transform if found.</param>
        /// <returns>Returns true if a GameObject with the specified tag is found, otherwise false.</returns>
        public static bool TryGetTransformWithTag(string tag, out Transform transform)
        {
            GameObject obj = GameObject.FindWithTag(tag);
            if (obj != null)
            {
                transform = obj.transform;
                return true;
            }
            else
            {
                transform = null;
                return false;
            }
        }

        /// <summary>
        /// Recursively changes the layer of the specified GameObject and all its descendants.
        /// </summary>
        /// <param name="obj">The GameObject whose layer is to be changed.</param>
        /// <param name="newLayer">The new layer to assign to the GameObject and its children.</param>
        public static void ChangeLayerRecursively(GameObject obj, int newLayer)
        {
            obj.layer = newLayer;
            foreach (Transform child in obj.transform)
            {
                ChangeLayerRecursively(child.gameObject, newLayer);
            }
        }

        /// <summary>
        /// Searches for a component of type T in the children of the specified GameObject.
        /// </summary>
        /// <typeparam name="T">The type of component to search for.</typeparam>
        /// <param name="parent">The parent GameObject in which to search for the component.</param>
        /// <param name="component">Output parameter that holds the found component.</param>
        /// <returns>Returns true if the component is found in a child, otherwise false.</returns>
        public static bool TryGetComponentInChild<T>(this GameObject parent, out T component) where T : Component
        {
            component = parent.GetComponentInChildren<T>();
            return component != null;
        }

        /// <summary>
        /// Finds all components of type T within the children of the specified parent GameObject.
        /// </summary>
        /// <typeparam name="T">The type of component to find.</typeparam>
        /// <param name="parent">The parent GameObject whose children are to be searched.</param>
        /// <returns>A list of all components of type T found in the children.</returns>
        public static List<T> GetComponentsInChildren<T>(this GameObject parent) where T : Component
        {
            List<T> components = new List<T>();
            T[] foundComponents = parent.GetComponentsInChildren<T>();

            if (foundComponents != null && foundComponents.Length > 0)
            {
                components.AddRange(foundComponents);
            }

            return components;
        }

        /// <summary>
        /// Determines if the specified GameObject is on the given layer.
        /// </summary>
        /// <param name="obj">The GameObject whose layer is to be compared.</param>
        /// <param name="layer">The layer to compare against the GameObject's layer.</param>
        /// <returns>Returns true if the GameObject is on the specified layer, otherwise false.</returns>
        public static bool CompareLayer(this GameObject obj, int layer)
        {
            return obj.layer == layer;
        }

        /// <summary>
        /// Determines if the specified GameObject is on the layer with the given name.
        /// </summary>
        /// <param name="obj">The GameObject whose layer is to be compared.</param>
        /// <param name="layerName">The name of the layer to compare against the GameObject's layer.</param>
        /// <returns>Returns true if the GameObject is on the specified layer, otherwise false.</returns>
        public static bool CompareLayer(this GameObject obj, string layerName)
        {
            return obj.layer == LayerMask.NameToLayer(layerName);
        }

        /// <summary>
        /// Checks if the specified GameObject has any children.
        /// </summary>
        /// <param name="obj">The GameObject to check for children.</param>
        /// <returns>Returns true if the GameObject has one or more children, otherwise false.</returns>
        public static bool HasChildren(GameObject obj)
        {
            return obj.transform.childCount > 0;
        }
    }
}