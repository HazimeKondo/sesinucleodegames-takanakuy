using System.Collections;

namespace Fader
{
    using UnityEngine;

    public class FadeEffect : MonoBehaviour
    {
        private Material _mat;
        
        private void Awake()
        {
            gameObject.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
            _mat = gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/AlwaysOnTop"));
            _mat.color = Color.clear;
            
            Camera camera = Camera.main;
            transform.SetParent(camera.transform);
            
            var nearClipPlane = camera.nearClipPlane;
            float height = 2.0f * nearClipPlane * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            
            transform.localPosition = Vector3.forward * (nearClipPlane+0.02f);
            transform.localScale = new Vector3(height * camera.aspect,height,1)*1.5f;
            transform.localRotation = Quaternion.identity;
        }

        public void StartFade(Color color, float duration)
        {
            StopAllCoroutines();
            StartCoroutine(FadeTo(color, duration));
        }

        private IEnumerator FadeTo(Color color, float duration)
        {
            Color originalColor = _mat.color;
            float timer = 0;

            while (timer <= duration)
            {
                timer += Time.deltaTime;
                _mat.color = Color.Lerp(originalColor, color, timer / duration);
                yield return null;
            }

            _mat.color = color;
        }
    }
}