using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



namespace LinkedListsScripts
{
    internal class Node //create linked list node to be used in the game
    {
        public Node(GameObject name, GameObject next)
        {
            _name = name;
            _data = UnityEngine.Random.Range(1, 100);
            _next = next;
        }
        
        public GameObject _name { get; set; }
        public int _data { get; set; }
        public GameObject _next { get; set; }
    }
    
    public class LinkedListGame2: MonoBehaviour
    {
        public GameObject node1;
        public GameObject node2;
        public GameObject node3;
        public GameObject node4;

        public GameObject topPanel;
        public GameObject bottomPanel;
        public GameObject feedbackPanel;

        public GameObject dropdown;

        private TextMeshProUGUI _topText;
        private TextMeshProUGUI _bottomText;
        
        public TextMeshProUGUI deleteText;
        public TextMeshProUGUI insertText;
        
        private Node _node1;
        private Node _node2;
        private Node _node3;
        private Node _node4;
        
        private GameObject _currentNode;
        
        private Dictionary<GameObject, Node> _dict = new();
        private int _counter = 0;
        
        public void SaveChanges()
        {
            var temp = dropdown.GetComponent<TMP_Dropdown>().options[dropdown.GetComponent<TMP_Dropdown>().value].text; //get the value of the dropdown
            
            if (temp == "Null Pointer") //if the value is null, set the next node to null
            {
                _dict[_currentNode]._next = null;
                UpdateLeftPanel(_currentNode);
                return;
            }
            
            var nodeObj = GameObject.Find(temp); //find the game-object with the name of the dropdown value
            _dict[_currentNode]._next = nodeObj;           //set the next node to the game-object
            UpdateLeftPanel(_currentNode);                //update the left panel to reflect the changes
        }

        public void SubmitAnswers()
        {
            switch (_counter) //check if the nodes are in the correct order and display feedback accordingly
            {
                case 0:
                    if (_dict[node1]._next == node2 && _dict[node2]._next == node4 && _dict[node3]._next == null && _dict[node4]._next == null)
                    {
                        feedbackPanel.SetActive(true);
                        feedbackPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Successfully delinked Node 3!";
                        (node3.transform.position, node4.transform.position) = (node4.transform.position, node3.transform.position);
                        _counter++;
                        deleteText.gameObject.SetActive(false);
                        insertText.gameObject.SetActive(true);
                        node3.GetComponent<Image>().color = Color.green;
                    } else {
                        feedbackPanel.SetActive(true);
                        feedbackPanel.GetComponentInChildren<TextMeshProUGUI>().text = "That's not quite it, have another look. Note that Node 3 should also point to Null and you must select save changes after each change.";
                    }
                    Invoke(nameof(ClearFeedback), 4);
                    break;
                case 1:
                    if (_dict[node1]._next == node3 && _dict[node2]._next == node4 && _dict[node3]._next == node2 && _dict[node4]._next == null)
                    {
                        feedbackPanel.SetActive(true);
                        feedbackPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Successfully inserted the new node!";
                        insertText.gameObject.SetActive(false);
                        _counter++;
                    } else {
                        feedbackPanel.SetActive(true);
                        feedbackPanel.GetComponentInChildren<TextMeshProUGUI>().text = "That's not quite it, have another look. Note that you must select save changes after each change.";
                    }
                    Invoke(nameof(ClearFeedback), 4);
                    break;
                default:
                    break;
            }

        }
        
        public void ClearFeedback() //clear the feedback panel
        {
            feedbackPanel.SetActive(false);
        }
        
        public void NextScene() //load linked list scenes
        {
            SceneManager.LoadScene("LinkedLists");
        }
        
        public void UpdateLeftPanel(GameObject node) //update the left panel to reflect the properties of each node
        {
            topPanel.SetActive(true);
            bottomPanel.SetActive(true);
            var tempNode = _dict[node];
            _topText.text = "Data stored in Node: " + tempNode._data;
            
            
            _currentNode = node;
            
            var options = new List<string>();
            foreach (var key in _dict.Keys)
            {
                if (key == node)
                {
                    continue;
                }
                
                options.Add(key.name);
            }
            options.Add("Null Pointer");
            
            dropdown.GetComponent<TMP_Dropdown>().ClearOptions();
            dropdown.GetComponent<TMP_Dropdown>().AddOptions(options);
            dropdown.GetComponent<TMP_Dropdown>().value = 0;
            dropdown.GetComponent<TMP_Dropdown>().RefreshShownValue();
            
            dropdown.GetComponent<TMP_Dropdown>().value = _dict[node]._next == null ? 0 : options.IndexOf(_dict[node]._next.name);
            
            
            
            if (tempNode._next != null)
            {
                _bottomText.text = "Next Node in List: " + tempNode._next.name;
            } else {
                _bottomText.text = "Next Node in List: Null Pointer";
            }
        }
        
        private void Update() //check if the game is complete, if so, display feedback
        {
            if (_counter != 2) return;
            topPanel.SetActive(false);
            bottomPanel.SetActive(false);
            feedbackPanel.SetActive(true);
            feedbackPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Congratulations! You have successfully completed the game!";
            Invoke(nameof(NextScene), 3);
        }

        private void Start() //initialise the nodes and the dictionary
        {
            _topText = topPanel.GetComponentInChildren<TextMeshProUGUI>();
            _bottomText = bottomPanel.GetComponentInChildren<TextMeshProUGUI>();
            
            _node1 = new Node(node1, node2);
            _node2 = new Node(node2, node3);
            _node3 = new Node(node3, node4);
            _node4 = new Node(node4, null);
            
            _dict = new Dictionary<GameObject, Node>
            {
                { node1, _node1 },
                { node2, _node2 },
                { node3, _node3 },
                { node4, _node4 }
            };
        }
    }
}