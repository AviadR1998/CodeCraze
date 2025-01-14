# CodeCrazeUnited: A Programming Adventure Game 🏝️

**CodeCrazeUnited** is an educational Unity-based game designed to teach children aged 7-14 fundamental programming concepts through interactive and engaging gameplay. Players explore a vibrant 3D world filled with diverse challenges that make learning programming fun and intuitive.
<br />

## Features

- **Interactive Learning**: The game provides hands-on experience with key programming concepts in an easy-to-understand format.
- **Gamified Environment**: A dynamic 3D world encourages exploration and active participation in solving programming challenges.
- **Progressive Design**: Tasks and topics increase in complexity as the player advances through the game.
<br />

## Installation and Execution 🛠️

1. Download the game files or extract the provided ZIP archive.
2. Navigate to the `executable` folder.
3. Double-click the file `codecrazeunited.exe` to launch the game.
<br />

## Controls 🕹️

- **Movement**: Use `W`, `A`, `S`, `D` keys to navigate the character through the environment.
- **Interactions**: Follow the on-screen instructions to complete each challenge.
<br />

## 🌟Project Vision🌟

**CodeCrazeUnited** aims to:
- Make learning programming accessible and enjoyable for children.
- Promote critical thinking and problem-solving skills.
- Provide an engaging platform to introduce foundational programming concepts.
  
<img width="728" alt="LOGO" src="https://github.com/user-attachments/assets/171145c0-41db-4c4f-9a4b-f08ff15613ee">
<br />

## Watch CodeCraze in Action 🎮
[![Watch the video](https://img.youtube.com/vi/r49FcDCghss/maxresdefault.jpg)](https://www.youtube.com/watch?v=r49FcDCghss)

<br />

## Architecture 🧩

The CodeCraze project employs a modular and scalable architecture, ensuring a seamless learning experience for young programmers. Below is an overview of the core components:
<br />

### 1. Client-Side (Unity3D)
- **Technology**: Unity3D with C# scripts.  
- **Purpose**: Manages the 3D game environment, user interactions, and educational missions.  
- **Features**:  
  - Real-time gameplay interactions and animations.  
  - Missions designed to teach Java programming basics.  
  - User authentication and data synchronization with the server.  


### 2. Server-Side (Node.js + Express.js)
- **Technology**: Node.js with the Express framework.  
- **Purpose**: Handles API requests, user management, and communication between the client and database.  
- **Features**:  
  - RESTful API endpoints for user data retrieval and storage.  
  - Secure authentication and session management.  
  - Data validation to ensure consistency.  


### 3. Database (MongoDB)
- **Technology**: MongoDB (NoSQL database).  
- **Purpose**: Stores user data, mission progress, and game configurations.  
- **Features**:  
  - User profiles, including usernames, passwords, and progress.  
  - Mission-specific data, such as completion status.  
  - Flexible schema for future scalability.  


### 4. Communication
- **Protocol**: HTTPS.  
- **Purpose**: Ensures secure data exchange between the client, server, and database.  
- **Features**:  
  - JSON-based API requests and responses. 


### 5. Development Environment
- **Local Deployment**: The project runs entirely on local machines for development and testing.  
- **Source Control**: Git and GitHub are used for version control and collaboration.  
- **Testing and Debugging**: All components were tested locally using debugging tools.  

