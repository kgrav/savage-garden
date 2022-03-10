using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI objectName;
    public LayerMask layerName;
    public Vector3 windowPos;
    public GameObject infoWindow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 1000f, layerName))
            {
                infoWindow.SetActive(true);
                Debug.Log(hit.collider.transform.root.GetComponent<MonsterBody>().monsterKey);
                var monsterKeyCast = hit.collider.transform.root.GetComponent<MonsterBody>().monsterKey;
                Debug.Log(Monsters.GetAffinityData(2));
                Debug.Log(hit.collider.transform.position);
                windowPos = hit.collider.transform.position;
                objectName.text = hit.collider.transform.root.name.ToString();
            } else {
                infoWindow.SetActive(false);
            }

            
        }

    }

}

