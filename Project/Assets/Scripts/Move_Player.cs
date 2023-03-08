using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Move_Player : MonoBehaviour
{
    PhotonView view;
    public TextMeshPro nick;

    [Header("Управление камерой")]
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;
    public Camera cam;

    [Header("Прыжок")]
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    public Vector3 playerScale;

    [Header("Скорость перемещения персонажа")]
    public float speed = 7f;

    [Header("Спавн и дестрой объектов.")]
    public GameObject Prefab;
    public GameObject aim;
    Ray spawnRay;
    RaycastHit hit;


    void Start()
    {
        view = GetComponent<PhotonView>();
        nick.GetComponent<TextMeshPro>().text = "Игрок";
    }

    void Update()
    {
        if (view.IsMine == true)
        {
            cam.gameObject.SetActive(true);
        }

        if (view.IsMine)
        {
           Cursor.lockState = CursorLockMode.Locked;
           Move();
           Shift();
           SpawnObject();
           DestroyObject();
        }

    }


    public void SpawnObject()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //Ray ray = cam.ScreenPointToRay(new Vector3(cam.transform.position.x, cam.transform.position.y, 1));
            //RaycastHit _hit;
            //if(Physics.Raycast(ray, out _hit, Mathf.Infinity))
            //{
            //    Instantiate(Prefab, new Vector3(ray.transform.position.x, ray.transform.position.y, ray.transform.position.z), Quaternion.identity);
            //}

            spawnRay = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast (spawnRay, out hit))
            {
                if(hit.transform.tag == "floor")
                {
                    //hit.point
                    PhotonNetwork.Instantiate("Cube", hit.point, Quaternion.identity);
                }
            } 
        }
    }

    public void DestroyObject()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            spawnRay = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(spawnRay, out hit))
            {
                if (hit.transform.tag == "object")
                {
                    PhotonNetwork.Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    private void Move()
    {
        CharacterController controller = GetComponent<CharacterController>();

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetKey(KeyCode.W))
            {
                transform.localPosition += transform.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.localPosition += -transform.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.localPosition += -transform.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.localPosition += transform.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void Shift()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 15f;
        }
        else
        {
            speed = 7f;
        }
    }
}
