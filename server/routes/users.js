import express from 'express'
import { addUser, returnUser, deleteUser, topScore } from '../controllers/users.js'
var usersRouter = express.Router();

usersRouter.post('/', addUser);
usersRouter.post('/AddScore', addScore);
usersRouter.get('/TopScore', topScore);
usersRouter.get('/:user', returnUser);
usersRouter.delete('/delete', deleteUser);

export default usersRouter;