using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("Инвентарь")]
    public GameObject[] blocks;
    public GameObject[] cellPanels;
    public int numPanels;
    public GameObject Torch;
    public GameObject Canvas;

    [Header("Звуки")]
    public AudioClip[] sound;
    public AudioSource music;

    void Start()
    {
        view = GetComponent<PhotonView>();
        nick.GetComponent<TextMeshPro>().text = "Игрок";
        numPanels = 0;
    }

    void Update()
    {
        if (view.IsMine == true)
        {
            cam.gameObject.SetActive(true);
            Canvas.gameObject.SetActive(true);
        }

        if (view.IsMine)
        {
           Cursor.lockState = CursorLockMode.Locked;
           Move();
           Shift();
           SpawnObject();
           DestroyObject();
           Inventory();
           TorchSetActive();
           esc_menu();
        }

    }


    public void SpawnObject()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            spawnRay = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast (spawnRay, out hit, 10.0f))
            {
                if (numPanels == 9)
                {
                    return;
                }
                else if (hit.transform.tag == "floor")
                {
                    PhotonNetwork.Instantiate(blocks[numPanels].name, hit.point, Quaternion.identity);
                    music.clip = sound[0];
                    music.Play();
                }
                else if(hit.transform.gameObject.name == "Trigger Y")
                {
                    PhotonNetwork.Instantiate(blocks[numPanels].name, hit.point = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z) + new Vector3(0, 0.459f, 0), Quaternion.identity);
                    music.clip = sound[0];
                    music.Play();
                }
                else if(hit.transform.gameObject.name == "Trigger -Y ")
                {
                    PhotonNetwork.Instantiate(blocks[numPanels].name, hit.point = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z) + new Vector3(0, -0.459f, 0), Quaternion.identity);
                    music.clip = sound[0];
                    music.Play();
                }
                else if (hit.transform.gameObject.name == "Trigger X")
                {
                    PhotonNetwork.Instantiate(blocks[numPanels].name, hit.point = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z) + new Vector3(0.459f, 0, 0), Quaternion.identity);
                    music.clip = sound[0];
                    music.Play();
                }
                else if (hit.transform.gameObject.name == "Trigger -X")
                {
                    PhotonNetwork.Instantiate(blocks[numPanels].name, hit.point = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z) + new Vector3(-0.459f, 0, 0), Quaternion.identity);
                    music.clip = sound[0];
                    music.Play();
                }
                else if (hit.transform.gameObject.name == "Trigger Z")
                {
                    PhotonNetwork.Instantiate(blocks[numPanels].name, hit.point = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z) + new Vector3(0, 0, 0.459f), Quaternion.identity);
                    music.clip = sound[0];
                    music.Play();
                }
                else if (hit.transform.gameObject.name == "Trigger -Z")
                {
                    PhotonNetwork.Instantiate(blocks[numPanels].name, hit.point = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z) + new Vector3(0, 0, -0.459f), Quaternion.identity);
                    music.clip = sound[0];
                    music.Play();
                }
            } 
        }
    }

    public void DestroyObject()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            spawnRay = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(spawnRay, out hit, 10.0f))
            {
                if (hit.transform.tag == "trigger")
                {
                    PhotonNetwork.Destroy(hit.transform.parent.gameObject);
                    music.clip = sound[1];
                    music.Play();
                }
            }
        }
    }

    public void Inventory()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (numPanels == 9)
            {
                cellPanels[numPanels].GetComponent<Image>().color = Color.gray;
                numPanels = 0;
                cellPanels[numPanels].GetComponent<Image>().color = Color.white;
            }
            else
            {
                cellPanels[numPanels].GetComponent<Image>().color = Color.gray;
                numPanels++;
                cellPanels[numPanels].GetComponent<Image>().color = Color.white;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (numPanels == 0)
            {
                cellPanels[numPanels].GetComponent<Image>().color = Color.gray;
                numPanels = 9;
                cellPanels[numPanels].GetComponent<Image>().color = Color.white;
            }
            else
            {
                cellPanels[numPanels].GetComponent<Image>().color = Color.gray;
                numPanels--;
                cellPanels[numPanels].GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void TorchSetActive()
    {
        if (numPanels == 9)
        {
            Torch.SetActive(true);
        }
        else
        {
            Torch.SetActive(false);
        }
    }

    public void esc_menu()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Confined;
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("LoadingScene");
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
