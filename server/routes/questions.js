import express from 'express'
import { returnQuestionsByLevel, returnQuestionsByTopic, addQuestion } from '../controllers/questions.js'
var questionsRouter = express.Router();

//questionsRouter.post('/', addQuestion);
questionsRouter.get('/Level/:level', returnQuestionsByLevel);
questionsRouter.get('/Topic/:topic', returnQuestionsByTopic);
questionsRouter.post('/Add', addQuestion);

export default questionsRouter;