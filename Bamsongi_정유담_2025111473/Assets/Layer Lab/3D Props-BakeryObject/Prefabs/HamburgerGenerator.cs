using UnityEngine;

public class HamburgerGenerator : MonoBehaviour
{
    public GameObject HamburgerPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject hamburger = Instantiate(HamburgerPrefab);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hamburger.GetComponent<HamburgerController>().Shoot(ray.direction * 2000);
        }
    }
}

