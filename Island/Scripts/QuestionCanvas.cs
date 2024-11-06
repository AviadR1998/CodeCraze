using System.Collections.Generic;
using System.IO;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionCanvas : MonoBehaviour
{
    public GameObject qCanvas, explainBtn;
    public Button[] buttons;
    public Button question, saveBtn;
    public TextMeshProUGUI responsePanel;
    public string csvPath;
    private int choosenOption, correctAns, numOfClicksOnSave = 0, currentQuestion = -1, numOfQ = 0;
    private string currentExplain;
    private List<Question> questions;
    private ColorBlock resetColor;


    // Start is called before the first frame update
    void Start()
    {
        makeQuestions(csvPath);
        resetColor = buttons[0].colors;
    }

    public class Question
    {
        public string QuestionText { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public int CorrectAnswer { get; set; }
        public string ExplainText { get; set; }
    }


    public void setColors()
    {

        if (choosenOption == 0)
        {
            return;
        }


        numOfClicksOnSave++;
        ColorBlock cb1 = buttons[0].colors;
        Color falseColor;
        UnityEngine.ColorUtility.TryParseHtmlString("#F23A3A", out falseColor);
        cb1.normalColor = falseColor;

        ColorBlock cb2 = buttons[0].colors;
        Color correctColor;
        UnityEngine.ColorUtility.TryParseHtmlString("#44C662", out correctColor);
        cb2.normalColor = correctColor;

        buttons[choosenOption - 1].colors = cb1;
        buttons[correctAns - 1].colors = cb2;

        if (choosenOption == correctAns)
        {
            responsePanel.text = "That's Right";
        }
        else
        {
            responsePanel.text = "Maybe Next Time";
        }

        if (numOfClicksOnSave % 2 == 1)
        {
            saveBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
            explainBtn.SetActive(true);
        }
        else
        {
            makeQuestions("");
        }

    }

    public void buttonClicked(int number)
    {
        choosenOption = number;
    }

    public void turnOffQuestion()
    {
        qCanvas.SetActive(false);
    }


    public class CsvReader
    {
        private readonly string filePath;

        public CsvReader(string filePath)
        {
            this.filePath = filePath;
        }

        public static string addNewLines(string str)
        {
            string retStr = "";
            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] != '~')
                {
                    retStr += str[i];
                    continue;
                }
                retStr += '\n';
            }

            return retStr;
        }

        public List<Question> LoadQuestions()
        {
            var questions = new List<Question>();

            using (var reader = new StreamReader(filePath))
            {
                // Skip the header line
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split("$,");
                    Console.WriteLine("The length of fields is: " + fields.Length);

                    if (fields.Length == 7)
                    {
                        try
                        {
                            int correctAnswer;
                            if (!int.TryParse(fields[5].Trim(), out correctAnswer))
                            {
                                Console.WriteLine($"Error parsing answer number in line: {line}");
                                continue; // Skip this entry if AnsNum is not an integer
                            }

                            var question = new Question
                            {
                                QuestionText = addNewLines(fields[0].Trim('"')),
                                Option1 = addNewLines(fields[1].Trim('"')),
                                Option2 = addNewLines(fields[2].Trim('"')),
                                Option3 = addNewLines(fields[3].Trim('"')),
                                Option4 = addNewLines(fields[4].Trim('"')),
                                CorrectAnswer = correctAnswer,
                                ExplainText = addNewLines(fields[6].Trim('"'))
                            };

                            questions.Add(question);
                            Console.WriteLine("In Try");
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing question in line: {line} - {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Line has incorrect format: {line}");
                    }
                }
            }

            return questions;
        }


    }


    public void makeQuestions(string csvPath)
    {
        if (currentQuestion == -1)
        {
            CsvReader csvReader = new CsvReader(csvPath);
            questions = csvReader.LoadQuestions();
            numOfQ = questions.Count;
        }
        ++currentQuestion;

        if (currentQuestion >= numOfQ)
        {
            turnOffQuestion();
            return;
        }


        if (choosenOption > 0)
        {
            buttons[choosenOption - 1].colors = resetColor;
            buttons[correctAns - 1].colors = resetColor;

            choosenOption = 0;
            explainBtn.SetActive(false);
            responsePanel.text = "";
            saveBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Save";
        }

        createQuestion();
    }

    public void createQuestion()
    {
        question.GetComponentInChildren<TextMeshProUGUI>().text = questions[currentQuestion].QuestionText;
        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = questions[currentQuestion].Option1;
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = questions[currentQuestion].Option2;
        buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = questions[currentQuestion].Option3;
        buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = questions[currentQuestion].Option4;

        currentExplain = questions[currentQuestion].ExplainText;
        correctAns = questions[currentQuestion].CorrectAnswer;
    }

    public void setExplaination()
    {
        question.GetComponentInChildren<TextMeshProUGUI>().text = currentExplain;
    }
}
