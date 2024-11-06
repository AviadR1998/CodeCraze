<b>Questions CSV File Guide</b>
The questions.csv file is used to load multiple-choice questions into the Unity quiz game. Each row represents a single question, with four possible answers, the correct answer's index, and an explanation.

<u>File Format Requirements</u>
Each row must follow this pattern:

question: The question text.
Op1: Text for answer option 1.
Op2: Text for answer option 2.
Op3: Text for answer option 3.
Op4: Text for answer option 4.
AnsNum: Index of the correct answer (1, 2, 3, or 4).
Explanation: Explanation of the correct answer.

<u>Row Formatting-</u>
End of Each Column: Every column ends with $ ,.
New Lines within Text: Use ~ for new lines instead of \n.
No Blank Rows: Avoid empty rows.
Unlimited Questions: Add as many rows as you like, following this pattern.

Example Row
******************************
What is the correct way to change the value at index 4 of this array int array = [1,2,3,4,5];?$,array = 4;$,array(4) = 1;$,array[4] = 1;$,array[1] = 4;$,3$,Correct syntax is array[4] = 1.$,
******************************

Notes:
Follow this format strictly to ensure the questions load correctly in the game.