import express from 'express'
import { returnExplanationsByTopic, addExplanations } from '../controllers/explanations.js'
var explanationsRouter = express.Router();

//questionsRouter.post('/', addQuestion);
explanationsRouter.get('/Topic/:topic', returnExplanationsByTopic);
explanationsRouter.post('/Add', addExplanations);

export default explanationsRouter;