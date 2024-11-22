import express from 'express';
import bodyParser from 'body-parser';
import usersRouter from './routes/users.js';
import tokensRouter from './routes/tokens.js';
import questionsRouter from './routes/questions.js';
import roomsRouter from './routes/rooms.js';
import explanationsRouter from './routes/explanations.js';
import { roomsList, getKey } from "./controllers/rooms.js";
//import { handleClientMsg } from './sockets/socket.js'
import cors from 'cors';
const app = express();

import http from 'http';
const server = http.createServer(app);
import { Server } from "socket.io";

var socketArr = new Map();
/*const io = new Server(server, {
    cors: { origin: "http://10.0.0.9:3000" }
});*/
const io = new Server(3000);

export const myIo = io;
export const arrSoc = socketArr;
io.on('connection', (socket) => {
    //console.log("hello");
    socket.on('username', (username) => {
        socketArr.set(socket, username);
        //console.log(username);
    })
    socket.on('disconnect', () => {
        if (roomsList.has(socketArr.get(socket))) {
            if (roomsList.get(socketArr.get(socket)) != "--") {
                io.to(socket).emit('host', null);
            }
        }
        socketArr.delete(socket);

    });

    socket.on('cord', (username, x, y, z, speed) => {
        console.log(x + "-" + y + "-" + z);
        getKey(socketArr, username).emit('cord', x, y, z, speed);
        //getKey(socketArr, username).emit('cord', x, y, z, speed);
        /*if (roomsList.has(username)) {
            getKey(socketArr, username).emit('cord', x, y, z);
        } else {
            getKey(socketArr, username).emit('cord', x, y, z);
        }*/
    });

    socket.on('finish', (username) => {
        console.log('finish');
        /*if (roomsList.has(username)) {
            getKey(socketArr, roomsList.get(username)).emit('finish', username);
        } else {
            getKey(socketArr, getKey(roomsList, username)).emit('finish', username);
        }*/
        roomsList.delete(username);
    });

    socket.on('disjoin', (username) => {
        roomsList.set(username, "--");
        getKey(arrSoc, username).emit("disjoin");
    });

});



app.use(cors());
app.use(bodyParser());
app.use('/api/Users', usersRouter);
app.use('/api/Tokens', tokensRouter);
app.use('/api/Questions', questionsRouter);
app.use('/api/Rooms', roomsRouter);
app.use('/api/Explanations', explanationsRouter);

// Catch-all middleware for undefined API routes
app.use((req, res, next) => {
    console.log(`Unhandled route: ${req.method} ${req.originalUrl}`);
    res.status(404).json({ error: "Not Found" });
});

// Serve static files (should come last)
app.use(express.static('public'));

app.listen(5000);