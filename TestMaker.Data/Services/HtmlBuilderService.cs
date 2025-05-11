using System.Text;
using TestMaker.Data.Models;
using TestMaker.Data.Services.ServiceModels;

namespace TestMaker.Data.Services;

public class HtmlBuilderService
{
    private readonly StringBuilder _stringBuilder = new();

    /// <summary>
    /// Generate head element of page
    /// </summary>
    /// <param name="language">Language page attribute</param>
    /// <param name="projectName">Title for page</param>
    /// <returns>HtmlBuilderService</returns>
    public HtmlBuilderService AddHead(string language, string projectName)
    {
        _stringBuilder.Append($"<!DOCTYPE html><html lang=\"{language}\">" +
                              "<head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">" +
                              $"<title>{projectName}</title>" +
                              "<style>:root{--background-color:#f4f4f4;--text-color:#000;--header-color:#fff;--header-background:#333;--button-background:#007BFF;--button-text-color:#fff;--question-background:#fff;--button-font-size:1.25em;--label-font-size:1.5em;--summary-font-size:1.20em;--openAnswer-font-size:1.15em}[data-theme=\"dark\"]{--background-color:#333;--text-color:#fff;--header-color:#fff;--header-background:#222;--button-background:#4CAF50;--button-text-color:#fff;--question-background:#444}body{font-family:Arial, sans-serif;margin:0;padding:0;display:flex;flex-direction:column;align-items:center;background-color:var(--background-color);color:var(--text-color)}header{background-color:var(--header-background);color:var(--header-color);padding:1em 0;text-align:center;width:100%;box-shadow:0 4px 8px rgb(0 0 0 / .2);position:relative}header h1{margin:0}header button{position:absolute;right:1em;top:50%;transform:translateY(-55%);background:none;border:none;cursor:pointer}#theme-icon{font-size:xx-large}header button svg{fill:var(--header-color)}.button-container{margin:1em 0;box-shadow:0 8px 16px rgb(0 0 0 / .2);display:flex}#checkAnswer,.button-container button{margin:0.5em;padding:0.5em 1em;background-color:var(--button-background);color:var(--button-text-color);border:none;cursor:pointer;font-size:var(--button-font-size);transition:box-shadow 0.3s ease;max-width:10em}#checkAnswer:hover,.button-container button:hover{box-shadow:0 4px 8px rgb(0 0 0 / .3)}.question-container{background-color:var(--question-background);padding:2em;border-radius:5px;box-shadow:0 8px 16px rgb(0 0 0 / .2);width:80%;max-width:600px}.question{margin-bottom:1em}.question label{font-size:var(--label-font-size)}#answer{display:flex;flex-direction:column}#answer label{font-size:var(--label-font-size);margin-top:0.25em;cursor:pointer}.borderWrong{border:2px solid red}.borderGood{border:2px solid green}.borderNeutral{border:2px solid var(--color-secondary)}summary{cursor:pointer}summary{cursor:pointer}.questionHPrefix{color:var(--button-background);font-size:1.5em}#questionH{font-size:1.5em}#answerContainer{display:flex;align-items:center;justify-content:space-between}#answerContainer div p{text-align:center}#answerContainer div:is(:nth-child(2)){display:none}#checkAnswerContainer{display:none}#detOdp{font-size:var(--openAnswer-font-size)}summary{font-size:var(--summary-font-size)}</style></head>");
        return this;
    }

    /// <summary>
    /// Generate body element with starting script element
    /// </summary>
    /// <param name="projectName">Test name</param>
    /// <param name="pageContent">Configuration of labels and buttons</param>
    /// <returns>HtmlBuilderService</returns>
    public HtmlBuilderService AddBody(string projectName, PageContent pageContent)
    {
        _stringBuilder.Append(
            "<body><header>" +
            $"<h1>{projectName}</h1>" +
            "<button id=\"theme-toggle\" aria-label=\"Toggle theme\"><span id=\"theme-icon\">\u2600\ufe0f</span></button></header>" +
            "<div class=\"button-container\">" +
            $"<button id=\"nextTO\">{pageContent.AnotherSingleChoiceQuestion}</button>" +
            $"<button id=\"randomTO\">{pageContent.RandomSingleChoiceQuestion}</button>" +
            $"<button id=\"nextTM\">{pageContent.AnotherMultipleChoiceQuestion}</button>" +
            $"<button id=\"randomTM\">{pageContent.RandomMultipleChoiceQuestion}</button>" +
            $"<button id=\"nextO\">{pageContent.AnotherOpenQuestion}</button>" +
            $"<button id=\"randomO\">{pageContent.RandomOpenQuestion}</button>" +
            "</div> <div class=\"question-container\"><div class=\"question\">" +
            $"<h3 class=\"questionHPrefix\" id=\"questionHeader\">{pageContent.QuestionHeader}</h3>" +
            "<h3 id=\"questionH\"></h3></div><div id=\"answerContainer\"><div>" +
            $"<h3 class=\"questionHPrefix\">{pageContent.Answer}</h3>" +
            "</div><div id=\"notAllContainer\">" +
            $"<p id=\"notAllP\">{pageContent.NotAllAnswers}</p>"+
            "</div><div id=\"checkAnswerContainer\" style=\"display: none;\"></div></div><div id=\"answer\"></div></div><script>");
        return this;
    }

    /// <summary>
    /// Generate script code that should always be on page
    /// </summary>
    /// <param name="pageContent">Configuration of labels and buttons</param>
    /// <returns>HtmlBuilderService</returns>
    public HtmlBuilderService AddScript(PageContent pageContent)
    {
        _stringBuilder.Append($"const questionHeaderText=\"{pageContent.QuestionHeader}\";const notAllAnswersText=\"{pageContent.NotAllAnswers}\";const testMultiAllAnswersText=\"{pageContent.TestMultiAllAnswers}\";const checkAnswerText=\"{pageContent.CheckAnswers}\";const showAnswerText=\"{pageContent.ShowAnswer}\";");
        _stringBuilder.Append(
            "function toggleTheme(){const body=document.body;const themeIcon=document.getElementById(\"theme-icon\");if(body.getAttribute(\"data-theme\")===\"dark\"){body.removeAttribute(\"data-theme\");themeIcon.textContent=\"\ud83c\udf19\"}else{body.setAttribute(\"data-theme\",\"dark\");themeIcon.textContent=\"\u2600\ufe0f\"}}document.getElementById(\"theme-toggle\").addEventListener(\"click\",toggleTheme);toggleTheme();function getRandomInt(max){min=0;max=Math.floor(max);return Math.floor(Math.random()*(max-min+1))+min}");
        return this;
    }

    /// <summary>
    /// Generate script code for test questions with only one correct answer
    /// </summary>
    /// <returns>HtmlBuilderService</returns>
    public HtmlBuilderService AddTestOneQuestionsScripts()
    {
        _stringBuilder.Append(
            "let nextTestOne=-1;let randomTestOne=0;let wasTestOne=[];document.getElementById(\"nextTO\").addEventListener(\"click\",function(){nextTestOne+=1;if(nextTestOne>testOneQuestions.length-1){nextTestOne=0}showTestOne(!0)});document.getElementById(\"randomTO\").addEventListener(\"click\",function(){if(wasTestOne.length>=testOneQuestions.length-5){wasTestOne=[]}randomTestOne=getRandomInt(testOneQuestions.length-1);while(wasTestOne.includes(randomTestOne)){randomTestOne=getRandomInt(testOneQuestions.length-1)}wasTestOne.push(randomTestOne);showTestOne(!1)});function showTestOne(isNext){document.getElementById(\"questionHeader\").innerText=questionHeaderText+(isNext?nextTestOne+1:randomTestOne+1);document.getElementById(\"checkAnswerContainer\").style.display=\"none\";document.getElementById(\"notAllContainer\").style.display=\"none\";document.getElementById(\"checkAnswer\")?.remove();const question=testOneQuestions[isNext?nextTestOne:randomTestOne];const answerElement=document.getElementById(\"answer\");answerElement.innerHTML=\"\";document.getElementById(\"questionH\").textContent=question.question;question.answers.forEach((answer,index)=>{const label=document.createElement(\"label\");label.id=\"label\"+index;label.className=\"borderNeutral\";const radio=document.createElement(\"input\");radio.type=\"radio\";radio.name=\"foo\";radio.style.marginLeft=\"1em\";radio.style.marginRight=\"1em\";radio.id=index;radio.addEventListener(\"click\",function(){const questionEl=testOneQuestions[isNext?nextTestOne:randomTestOne];questionEl.answers.forEach((_,id)=>{const labelString=\"label\"+id;const otherLabel=document.getElementById(labelString);if(id!=this.id){otherLabel.classList.remove(\"borderGood\",\"borderWrong\");otherLabel.classList.add(\"borderNeutral\")}});const answer=questionEl.answers[this.id];label.classList.remove(\"borderNeutral\",\"borderGood\",\"borderWrong\");if(answer.correct){label.classList.add(\"borderGood\")}else{label.classList.add(\"borderWrong\");for(let i=0;i<questionEl.answers.length;i+=1){if(questionEl.answers[i].correct){const labelString=\"label\"+i;const otherLabel=document.getElementById(labelString);otherLabel.classList.remove(\"borderNeutral\",\"borderGood\",\"borderWrong\");otherLabel.classList.add(\"borderGood\")}}}});label.appendChild(radio);label.append(answer.text);answerElement.appendChild(label)});document.querySelector(\".question-container\").style.display=\"block\"}");
        return this;
    }

    /// <summary>
    /// Generate script code for test questions with multiple correct answers
    /// </summary>
    /// <returns>HtmlBuilderService</returns>
    public HtmlBuilderService AddTestMultiQuestionsScripts()
    {
        _stringBuilder.Append(
            "const multi_selected=[];let nextTestMulti=-1;let randomTestMulti=0;let wasTestMulti=[];document.getElementById(\"nextTM\").addEventListener(\"click\",function(){nextTestMulti+=1;if(nextTestMulti>testMultiQuestions.length-1){nextTestMulti=0}showTestMulti(!0)});document.getElementById(\"randomTM\").addEventListener(\"click\",function(){if(wasTestMulti.length>=testMultiQuestions.length-2){wasTestMulti=[]}randomTestMulti=getRandomInt(testMultiQuestions.length-1);while(wasTestMulti.includes(randomTestMulti)){randomTestMulti=getRandomInt(testMultiQuestions.length-1)}wasTestMulti.push(randomTestMulti);showTestMulti(!1)});function showTestMulti(isNext){document.getElementById(\"notAllContainer\").style.display=\"block\";if(multi_selected.length>0){multi_selected.length=0}document.getElementById(\"questionHeader\").innerText=questionHeaderText+(isNext?nextTestMulti+1:randomTestMulti+1);document.getElementById(\"checkAnswerContainer\").style.display=\"block\";document.getElementById(\"checkAnswer\")?.remove();const question=testMultiQuestions[isNext?nextTestMulti:randomTestMulti];const answerElement=document.getElementById(\"answer\");answerElement.innerHTML=\"\";document.getElementById(\"questionH\").textContent=question.question;question.answers.forEach((answer,index)=>{const label=document.createElement(\"label\");label.id=\"label\"+index;label.className=\"borderNeutral\";const checkbox=document.createElement(\"input\");checkbox.type=\"checkbox\";checkbox.name=\"foo\";checkbox.style.marginLeft=\"1em\";checkbox.style.marginRight=\"1em\";checkbox.id=index;checkbox.addEventListener(\"click\",function(){if(this.checked){multi_selected.push(this.id)}else{const index=multi_selected.indexOf(this.id);if(index>-1){multi_selected.splice(index,1)}}});label.appendChild(checkbox);label.append(answer.text);answerElement.appendChild(label)});const checkAnswer=document.createElement(\"button\");checkAnswer.textContent=checkAnswerText;checkAnswer.style.marginTop=\"1em\";checkAnswer.style.marginBottom=\"1em\";checkAnswer.style.marginLeft=\"1em\";checkAnswer.style.marginRight=\"1em\";checkAnswer.id=\"checkAnswer\";checkAnswer.addEventListener(\"click\",function(){const questionEl=testMultiQuestions[isNext?nextTestMulti:randomTestMulti];questionEl.answers.forEach((_,id)=>{const label=document.getElementById(\"label\"+id);if(label.firstChild.checked){if(questionEl.answers[id].correct){label.classList.remove(\"borderNeutral\",\"borderGood\",\"borderWrong\");label.classList.add(\"borderGood\")}else{label.classList.remove(\"borderNeutral\",\"borderGood\",\"borderWrong\");label.classList.add(\"borderWrong\")}}else{label.classList.remove(\"borderNeutral\",\"borderGood\",\"borderWrong\");label.classList.add(\"borderNeutral\")}});let correctAnswers=0;let allCorrectChecked=true;for(let i=0;i<questionEl.answers.length;i+=1){if(questionEl.answers[i].correct&&!multi_selected.includes(i.toString())){allCorrectChecked=false}else if(questionEl.answers[i].correct&&multi_selected.includes(i.toString())){correctAnswers+=1}if(!questionEl.answers[i].correct&&multi_selected.includes(i.toString())){allCorrectChecked=false}}let correctAnswersFromQuestion=questionEl.answers.filter((answer)=>answer.correct).length;if(!allCorrectChecked){document.getElementById(\"notAllP\").innerText=notAllAnswersText+(correctAnswers+\" / \"+correctAnswersFromQuestion);document.getElementById(\"notAllContainer\").style.display=\"block\"}else{document.getElementById(\"notAllP\").innerText=testMultiAllAnswersText;document.getElementById(\"notAllContainer\").style.display=\"block\"}});document.getElementById(\"checkAnswerContainer\").appendChild(checkAnswer);document.querySelector(\".question-container\").style.display=\"block\"}");
        return this;
    }

    /// <summary>
    /// Generate script code for open questions
    /// </summary>
    /// <returns>HtmlBuilderService</returns>
    public HtmlBuilderService AddOpenQuestionsScript()
    {
        _stringBuilder.Append(
            "let nextOpen=-1;let randomOpen=0;let wasOpen=[];document.getElementById(\"nextO\").addEventListener(\"click\",function(){nextOpen+=1;if(nextOpen>openQuestions.length-1){nextOpen=0}showOpen(!0)});document.getElementById(\"randomO\").addEventListener(\"click\",function(){if(wasOpen.length>=openQuestions.length-2){wasOpen=[]}randomOpen=getRandomInt(openQuestions.length-1);while(wasOpen.includes(randomOpen)){randomOpen=getRandomInt(openQuestions.length-1)}wasOpen.push(randomOpen);showOpen(!1)});function showOpen(isNext){document.getElementById(\"questionHeader\").innerText=questionHeaderText+(isNext?nextOpen+1:randomOpen+1);document.getElementById(\"checkAnswerContainer\").style.display=\"none\";document.getElementById(\"notAllContainer\").style.display=\"none\";document.getElementById(\"checkAnswer\")?.remove();const question=openQuestions[isNext?nextOpen:randomOpen];const questionH=document.getElementById(\"questionH\");const answerElement=document.getElementById(\"answer\");questionH.innerHTML=\"\";answerElement.innerHTML=\"\";questionH.textContent=question.question;const details=document.createElement(\"details\");const summary=document.createElement(\"summary\");summary.textContent=showAnswerText;const p=document.createElement(\"p\");p.id=\"detOdp\";p.textContent=question.answer;details.appendChild(summary);details.appendChild(p);answerElement.appendChild(details);document.querySelector(\".question-container\").style.display=\"block\"}");
        return this;
    }



    /// <summary>
    /// Generate JSON array of questions
    /// </summary>
    /// <param name="questions">List of all questions from test</param>
    /// <returns>HtmlBuilderService</returns>
    public HtmlBuilderService AddQuestions(List<Question> questions)
    {
        var testOneQuestions = questions.OfType<TestOneQuestion>().ToList();
        var testMultiQuestions = questions.OfType<TestMultiQuestion>().ToList();
        var openQuestions = questions.OfType<OpenQuestion>().ToList();

        if (testOneQuestions.Count != 0)
        {
            // const testOneQuestions = [

            _stringBuilder.Append("const testOneQuestions = [");

            // [
            //      {
            //          question: "Foo bar bazz",
            //          answers: [
            //          {
            //              text: "Correct Answer A",
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
                    _stringBuilder.Append($"{{ text: \"{a.Answer.Value}\", correct: ");
                    _stringBuilder.Append(a.AnswerValue == q.CorrectAnswer ? "true" : "false");
                    _stringBuilder.Append("},");
                });
                _stringBuilder.Append("]},");
            });

            // ];

            _stringBuilder.Append("];");
        }

        if (testMultiQuestions.Count != 0)
        {
            // const testMultiQuestions = [

            _stringBuilder.Append("const testMultiQuestions = [");

            // {
            //     question: "Foo bar bazz",
            //     answers: [
            //     {
            //         text: "Correct Answer A",
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
                    _stringBuilder.Append($"{{ text: \"{a.Answer.Value}\", correct: ");
                    _stringBuilder.Append(q.CorrectAnswers.Contains(a.AnswerValue) ? "true" : "false");
                    _stringBuilder.Append("},");
                });
                _stringBuilder.Append("]},");
            });

            // ];

            _stringBuilder.Append("];");
        }

        if (openQuestions.Count == 0) return this;

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
            _stringBuilder.Append($"{{ question: \"{q.QuestionText}\", answer: \"{q.Answer.Value}\"}},");
        });
        _stringBuilder.Append("];");

        return this;
    }

    /// <summary>
    /// Generate closing:
    /// - script
    /// - body
    /// - html element
    /// </summary>
    /// <returns>Complete HTML page</returns>
    public string Collect()
    {
        _stringBuilder.Append("</script>" +
                              "</body>" +
                              "</html>");
        return _stringBuilder.ToString();
    }
}