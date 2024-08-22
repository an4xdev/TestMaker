using System.Text;
using TestMaker.Data.Models;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Data.Services;

public class HtmlBuilderService
{
    private readonly StringBuilder _stringBuilder = new();

    public HtmlBuilderService AddHead(string language, string projectName)
    {
        _stringBuilder.Append($"<!DOCTYPE html><html lang=\"{language}\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>{projectName}</title><style> :root{{--background-color:#f4f4f4;--text-color:#000;--header-color:#fff;--header-background:#333;--button-background:#007BFF;--button-text-color:#fff;--question-background:#fff;--button-font-size:1.25em;--label-font-size:1.5em}}[data-theme=\"dark\"]{{--background-color:#333;--text-color:#fff;--header-color:#fff;--header-background:#222;--button-background:#4CAF50;--button-text-color:#fff;--question-background:#444}}body{{font-family:Arial,sans-serif;margin:0;padding:0;display:flex;flex-direction:column;align-items:center;background-color:var(--background-color);color:var(--text-color)}}header{{background-color:var(--header-background);color:var(--header-color);padding:1em 0;text-align:center;width:100%;box-shadow:0 4px 8px rgb(0 0 0 / .2);position:relative}}header h1{{margin:0}}header button{{position:absolute;right:1em;top:50%;transform:translateY(-55%);background:none;border:none;cursor:pointer}}#theme-icon{{font-size:xx-large}}header button svg{{fill:var(--header-color)}}.button-container{{margin:1em 0;box-shadow:0 8px 16px rgb(0 0 0 / .2);display:flex}}.button-container button{{margin:.5em;padding:.5em 1em;background-color:var(--button-background);color:var(--button-text-color);border:none;cursor:pointer;font-size:var(--button-font-size);transition:box-shadow 0.3s ease;max-width:10em}}.button-container button:hover{{box-shadow:0 4px 8px rgb(0 0 0 / .3)}}.question-container{{background-color:var(--question-background);padding:2em;border-radius:5px;box-shadow:0 8px 16px rgb(0 0 0 / .2);width:80%;max-width:600px}}.question{{margin-bottom:1em}}.question label{{font-size:var(--label-font-size)}}#answer{{display:flex;flex-direction:column}}#answer label{{font-size:var(--label-font-size);margin-top:.25em;cursor:pointer}}.theme-toggle{{margin:1em;padding:.5em 1em;background-color:var(--button-background);color:var(--button-text-color);border:none;cursor:pointer;font-size:var(--button-font-size)}}.borderWrong{{border:2px solid red}}.borderGood{{border:2px solid green}}.borderNeutral{{border:2px solid var(--color-secondary)}}summary{{cursor:pointer}}summary {{cursor: pointer}}.questionHPrefix {{color: var(--button-background);font-size: 1.5em;}}#questionH {{font-size: 1.5em;}}</style></head>");
        return this;
    }

    public HtmlBuilderService AddBody(string projectName, PageContent pageContent)
    {
        _stringBuilder.Append(
            $"<body><header><h1>{projectName}</h1><button id=\"theme-toggle\" aria-label=\"Toggle theme\"><span id=\"theme-icon\">\u2600\ufe0f</span></button></header><div class=\"button-container\"><button id=\"nextTO\">{pageContent.Button1}</button><button id=\"randomTO\">{pageContent.Button2}</button><button id=\"nextTM\">{pageContent.Button3}</button><button id=\"randomTM\">{pageContent.Button4}</button><button id=\"nextO\">{pageContent.Button5}</button><button id=\"randomO\">{pageContent.Button6}</button></div><div class=\"question-container\"><div class=\"question\"><h3 class=\"questionHPrefix\">Question:</h3><h3 id=\"questionH\"></h3></div><div><h3 class=\"questionHPrefix\">Answer:</h3></div><div id=\"answer\"></div></div>");
        return this;
    }

    public HtmlBuilderService AddScript(string showOpenQuestionText)
    {
        _stringBuilder.Append($"<script>let nextTestOne=-1;let randomTestOne=0;let nextTestMulti=-1;let randomTestMulti=0;let nextOpen=-1;let randomOpen=0;let wasTestOne=[];let wasTestMulti=[];let wasOpen=[];function toggleTheme(){{const body=document.body;const themeIcon=document.getElementById(\"theme-icon\");if(body.getAttribute(\"data-theme\")===\"dark\"){{body.removeAttribute(\"data-theme\");themeIcon.textContent=\"\ud83c\udf19\"}}else{{body.setAttribute(\"data-theme\",\"dark\");themeIcon.textContent=\"\u2600\ufe0f\"}}}}document.getElementById(\"theme-toggle\").addEventListener(\"click\",toggleTheme);document.addEventListener(\"DOMContentLoaded\",function(){{toggleTheme();document.getElementById(\"nextTO\").addEventListener(\"click\",function(){{nextTestOne++;if(nextTestOne>testOneQuestions.length-1){{nextTestOne=0}}showTestOne(!0)}});document.getElementById(\"randomTO\").addEventListener(\"click\",function(){{if(wasTestOne.length>=testOneQuestions.length-5){{wasTestOne=[]}}randomTestOne=getRandomInt(0,testOneQuestions.length-1);while(wasTestOne.includes(randomTestOne)){{randomTestOne=getRandomInt(0,testOneQuestions.length-1)}}wasTestOne.push(randomTestOne);showTestOne(!1)}});document.getElementById(\"nextTM\").addEventListener(\"click\",function(){{nextTestMulti++;if(nextTestMulti>testMultiQuestions.length-1){{nextTestMulti=0}}showTestMulti(!0)}});document.getElementById(\"randomTM\").addEventListener(\"click\",function(){{if(wasTestMulti.length>=testMultiQuestions.length-2){{wasTestMulti=[]}}randomTestMulti=getRandomInt(0,testMultiQuestions.length-1);while(wasTestMulti.includes(randomTestMulti)){{randomTestMulti=getRandomInt(0,testMultiQuestions.length-1)}}wasTestMulti.push(randomTestMulti);showTestMulti(!1)}});document.getElementById(\"nextO\").addEventListener(\"click\",function(){{nextOpen++;if(nextOpen>openQuestions.length-1){{nextOpen=0}}showOpen(!0)}});document.getElementById(\"randomO\").addEventListener(\"click\",function(){{if(wasOpen.length>=openQuestions.length-2){{wasOpen=[]}}randomOpen=getRandomInt(0,openQuestions.length-1);while(wasOpen.includes(randomOpen)){{randomOpen=getRandomInt(0,openQuestions.length-1)}}wasOpen.push(randomOpen);showOpen(!1)}})}});function showTestOne(isNext){{const question=testOneQuestions[isNext?nextTestOne:randomTestOne];const answerElement=document.getElementById(\"answer\");answerElement.innerHTML=\"\";document.getElementById(\"questionH\").textContent=question.question;question.answers.forEach((answer,index)=>{{const label=document.createElement(\"label\");label.id=`label${{index}}`;label.className=\"borderNeutral\";const radio=document.createElement(\"input\");radio.type=\"radio\";radio.name=\"foo\";radio.style.marginLeft=\"1em\";radio.style.marginRight=\"1em\";radio.id=index;radio.addEventListener(\"click\",function(){{const questionEl=testOneQuestions[isNext?nextTestOne:randomTestOne];const answer=questionEl.answers[this.id];const labelID=`label${{this.id}}`;label.classList.remove(\"borderNeutral\",\"borderGood\",\"borderWrong\");if(answer.correct){{label.classList.add(\"borderGood\")}}else{{label.classList.add(\"borderWrong\")}}questionEl.answers.forEach((_,id)=>{{const otherLabel=document.getElementById(`label${{id}}`);if(id!=this.id){{otherLabel.classList.remove(\"borderGood\",\"borderWrong\");otherLabel.classList.add(\"borderNeutral\")}}}})}});label.appendChild(radio);label.append(answer.text);answerElement.appendChild(label)}});document.querySelector(\".question-container\").style.display=\"block\"}}function showTestMulti(isNext){{const question=testMultiQuestions[isNext?nextTestMulti:randomTestMulti];const answerElement=document.getElementById(\"answer\");answerElement.innerHTML=\"\";document.getElementById(\"questionH\").textContent=question.question;question.answers.forEach((answer,index)=>{{const label=document.createElement(\"label\");label.id=`label${{index}}`;label.className=\"borderNeutral\";const checkbox=document.createElement(\"input\");checkbox.type=\"checkbox\";checkbox.name=\"foo\";checkbox.style.marginLeft=\"1em\";checkbox.style.marginRight=\"1em\";checkbox.id=index;checkbox.addEventListener(\"click\",function(){{const questionEl=testMultiQuestions[isNext?nextTestMulti:nextTestMulti];const answer=questionEl.answers[this.id];const label=document.getElementById(`label${{this.id}}`);label.classList.remove(\"borderNeutral\",\"borderGood\",\"borderWrong\");if(this.checked){{if(answer.correct){{label.classList.add(\"borderGood\")}}else{{label.classList.add(\"borderWrong\")}}}}else{{label.classList.add(\"borderNeutral\")}}}});label.appendChild(checkbox);label.append(answer.text);answerElement.appendChild(label)}});document.querySelector(\".question-container\").style.display=\"block\"}}function showOpen(isNext){{const question=openQuestions[isNext?nextOpen:randomOpen];const questionH=document.getElementById(\"questionH\");const answerElement=document.getElementById(\"answer\");questionH.innerHTML=\"\";answerElement.innerHTML=\"\";questionH.textContent=question.question;const details=document.createElement(\"details\");const summary=document.createElement(\"summary\");summary.textContent=\"{showOpenQuestionText}\";const p=document.createElement(\"p\");p.id=\"detOdp\";p.textContent=question.answer;details.appendChild(summary);details.appendChild(p);answerElement.appendChild(details);document.querySelector(\".question-container\").style.display=\"block\"}}function getRandomInt(min,max){{min=Math.ceil(min);max=Math.floor(max);return Math.floor(Math.random()*(max-min+1))+min}}");
        return this;
    }

    public HtmlBuilderService AddQuestions(List<Question> questions)
    {
        var testOneQuestions = questions.OfType<TestOneQuestion>().ToList();
        var testMultiQuestions = questions.OfType<TestMultiQuestion>().ToList();
        var openQuestions = questions.OfType<OpenQuestion>().ToList();
        
        // const testOneQuestions = [
        
        _stringBuilder.Append("const testOneQuestions = [");
        
        // [
        //      {
        //          question: "Foo bar bazz",
        //          answers: [
        //          {
        //              text: "Corrent Answer A",
        //              correct: true,
        //          },
        //          {
        //              text: "Answer B",
        //              correct: false,
        //          },
        //          {
        //              text: "Answer C",
        //              correct: false,
        //          },
        //          {
        //              text: "Answer D",
        //              correct: false,
        //          },
        //          ],
        //      },
        // ];
        testOneQuestions.ForEach(q =>
        {
            _stringBuilder.Append($"{{ question: \"{q.QuestionText}\", answers: [");
            q.Answers.ForEach(a =>
            {
                _stringBuilder.Append($"{{ text: \"{a.Answer}\", correct: ");
                _stringBuilder.Append(a.AnswerValue == q.CorrectAnswer ? "true" : "false");
                _stringBuilder.Append("},");
            });
            _stringBuilder.Append("]},");
        });
        
        // ];
        
        _stringBuilder.Append("];");
        
        // const testMultiQuestions = [
        
        _stringBuilder.Append("const testMultiQuestions = [");
        
        // {
        //     question: "Foo bar bazz",
        //     answers: [
        //     {
        //         text: "Corrent Answer A",
        //         correct: true,
        //     },
        //     {
        //         text: "Answer B",
        //         correct: false,
        //     },
        //     {
        //         text: "Correct Answer C",
        //         correct: true,
        //     },
        //     {
        //         text: "Answer D",
        //         correct: false,
        //     },
        //     ],
        // },
        
        testMultiQuestions.ForEach(q =>
        {
            _stringBuilder.Append($"{{ question: \"{q.QuestionText}\", answers: [");
            q.Answers.ForEach(a =>
            {
                _stringBuilder.Append($"{{ text: \"{a.Answer}\", correct: ");
                _stringBuilder.Append(q.CorrectAnswers.Contains(a.AnswerValue) ? "true" : "false");
                _stringBuilder.Append("},");
            });
            _stringBuilder.Append("]},");
        });
        
        // ];
        
        _stringBuilder.Append("];");
        
        // const openQuestions = [
        //      {
        //          question: "Foo bar bazz",
        //          answer:
        //          "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Aliquid dolore corporis alias laudantium architecto inventore, ex non magnam aliquam assumenda nesciunt est iste eos deleniti et fugiat sit aut dolorum.",
        //      },
        // ];
        
        _stringBuilder.Append("const openQuestions = [");
        
        openQuestions.ForEach(q =>
        {
            _stringBuilder.Append($"{{ question: \"{q.QuestionText}\", answer: \"{q.Answer}\"}},");
        });
        _stringBuilder.Append("];");
        
        return this;
    }

    public string Collect()
    {
        _stringBuilder.Append("</script></body></html>");
        return _stringBuilder.ToString();
    }
}