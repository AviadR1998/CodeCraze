import express from 'express'
import { returnQuestionsByLevel, returnQuestionsByTag } from '../controllers/questions.js'
var questionsRouter = express.Router();

questionsRouter.get('/Level/:level', returnQuestionsByLevel);
questionsRouter.get('/Tag/:tag', returnQuestionsByTag);

export default questionsRouter;