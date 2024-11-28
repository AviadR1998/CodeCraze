import express from 'express'
import { addUser, returnUser, deleteUser, resetUser, topScore, addScore, saveState, getState } from '../controllers/users.js'
var usersRouter = express.Router();

usersRouter.post('/', addUser);
usersRouter.post('/AddScore', addScore);
usersRouter.get('/TopScore', topScore);
usersRouter.delete('/Delete', deleteUser);
usersRouter.post('/Reset', resetUser);
usersRouter.post('/SaveState', saveState);
usersRouter.get('/GetState', getState);

usersRouter.get('/:user', returnUser);

export default usersRouter;