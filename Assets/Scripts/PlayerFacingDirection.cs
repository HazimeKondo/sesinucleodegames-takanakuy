using UnityEngine;

public class PlayerFacingDirection : MonoBehaviour {

    private Camera cam;
    public Vector3 point;
    private GameObject go;
    public Vector2 mousePosNormal;
    private Vector2 screenDimensions;
    private float goY;

    private void Awake() {
        go = new GameObject();
        go.transform.position = transform.position;
        cam = Camera.main;
        screenDimensions = new Vector2(Screen.width, Screen.height);
        goY = go.transform.localPosition.y;
    }
    private void Update() {

        mousePosNormal = new Vector2(Input.mousePosition.x / screenDimensions.x, Input.mousePosition.y / screenDimensions.y);

        Ray ray = cam.ViewportPointToRay(new Vector3(mousePosNormal.x, mousePosNormal.y, 0));
        LayerMask mask = LayerMask.GetMask("MousePositionCapture");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, mask.value)) {
            point = hit.point;
        }
        point = new Vector3(point.x, goY, point.z);
        transform.LookAt(point);
    }
}
