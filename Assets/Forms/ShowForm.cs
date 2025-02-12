using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class ShowForm : MonoBehaviour
{
    [Serializable]
    private class Question
    {
        public string QuestionText;
        public bool CanBeAnswered = true;
        public string AnswerText;
        public bool PreviousButton = true;
        public GameObject buttonSelected;
    }

    [SerializeField] private List<Question> questionsAndPrompts;
    [SerializeField] private TMP_Text textPrompt;
    [SerializeField] private GameObject Answers;
    [SerializeField] private GameObject NextButton;
    [SerializeField] private GameObject PreviousButton;
    [Tooltip("Keeps the first and last prompt in same place, randomize the rest.")]
    [SerializeField] private bool randomOrder;
    private int index = 0;

    private void Start()
    {
        textPrompt.text = questionsAndPrompts[0].QuestionText;
        Answers.SetActive(questionsAndPrompts[index].CanBeAnswered);
        PreviousButton.SetActive(false);
        if(randomOrder)RandomizeOrder(questionsAndPrompts);
    }

    public void Next()
    {
        if(questionsAndPrompts[index].buttonSelected == null && questionsAndPrompts[index].CanBeAnswered)return;
        index++;
        textPrompt.text = questionsAndPrompts[index].QuestionText;
        Answers.SetActive(questionsAndPrompts[index].CanBeAnswered);
        NextButton.SetActive(index != questionsAndPrompts.Count - 1);
        if (questionsAndPrompts[index].buttonSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(questionsAndPrompts[index].buttonSelected);
        }
        PreviousButton.SetActive(questionsAndPrompts[index].PreviousButton);
    }

    public void Previous()
    {
        index--;
        textPrompt.text = questionsAndPrompts[index].QuestionText;
        Answers.SetActive(questionsAndPrompts[index].CanBeAnswered);
        if (questionsAndPrompts[index].buttonSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(questionsAndPrompts[index].buttonSelected);
        }
        if(index == 0)PreviousButton.SetActive(false);
        NextButton.SetActive(true);
    }

    public void OnSelect()
    {
        questionsAndPrompts[index].AnswerText = 
            EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;
        questionsAndPrompts[index].buttonSelected = EventSystem.current.currentSelectedGameObject;
        
    }

    private void RandomizeOrder<T>(List<T> ts)
    {
        var count = ts.Count;
        var last = count - 2;
        for (var i = 1; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count - 1);
            (ts[i], ts[r]) = (ts[r], ts[i]);
        }
    }
}
