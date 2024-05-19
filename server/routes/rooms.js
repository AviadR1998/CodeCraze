import express from 'express'
import { returnAllRooms, createRoom, removeRoom, joinRoom } from '../controllers/rooms.js'
var roomsRouter = express.Router();

roomsRouter.get('/', returnAllRooms);
roomsRouter.post('/Create/:username', createRoom);
roomsRouter.post('/Join/:username', joinRoom);
roomsRouter.delete('/Delete/:username', removeRoom)

export default roomsRouter;