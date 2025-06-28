#include <iostream>

using namespace std;

int main() {
    int state = 1; 
    string direction;

    bool hasTorch = false;
    bool hasKey5 = false;
    bool hasKey6 = false;

    cout << "Welcome to the Maze game...\n";
    cout << "There's some strange rooms...\n";
    cout << "Find the objects to enter every room, good luck!\n";

room1:
    if (hasTorch && hasKey5 && hasKey6) {
        cout << "Congratulations! You have collected all items.\n";
        cout << "A secret door opens... YOU ESCAPE THE MAZE! \n";
        return 0; 
    }

    cout << "Room 1: There's a dark room with only one door. You can go East (E).\n";
    cin >> direction;
    if (direction == "E") {
        goto room2;
    } else {
        cout << "You can't go that direction, try again...\n";
        goto room1;
    }

room2:
    cout << "Room 2: You can go North-West (NW), East (E) or South-East (SE). You can also return West (W).\n";
    cin >> direction;
    if (direction == "NW") {
        goto room3;
    } else if (direction == "E") {
        goto room5;
    } else if (direction == "SE") {
        goto room8;
    } else if (direction == "W") {
        goto room1;
    } else {
        cout << "Invalid direction. Try again...\n";
        goto room2;
    }

room3:
    if (!hasTorch) {
        cout << "It's too dark, you can't see anything. Try and find a light source first.\n";
        goto room2; 
    }
    cout << "Room 3: With the torch, you can see a door to the North (N) or move to (NW).\n";
    cin >> direction;
    if (direction == "N") {
        goto room4;
    } else if (direction == "NW") {
        goto room2;
    } else {
        cout << "Invalid direction.\n";
        goto room3;
    }

room4:
    if (!hasKey5) {
        cout << "Room 4: You found a key for Room 5!\n";
        hasKey5 = true;
    }
    cout << "Go back South (S)?\n";
    cin >> direction;
    if (direction == "S") {
        goto room3;
    } else {
        cout << "Invalid direction.\n";
        goto room4;
    }

room5:
    if (!hasKey5) {
        cout << "The door to Room 5 is locked. You need a key.\n";
        goto room2; 
    }
    cout << "Room 5: You unlocked the door with Key 5. You can go North-East (NE) or South-East (SE). You can also return West (W).\n";
    cin >> direction;
    if (direction == "NE") {
        goto room6;
    } else if (direction == "SE") {
        goto room7;
    } else if (direction == "W") {
        goto room2;
    } else {
        cout << "Invalid direction.\n";
        goto room5;
    }

room6:
    if (!hasKey6) {
        cout << "Room 6 is locked. You need Key 6.\n";
        goto room5;
    }
    cout << "Room 6: You unlocked the door with Key 6. There's nothing else here.\n";
    cout << "Go back West (W)?\n";
    cin >> direction;
    if (direction == "W") {
        goto room5;
    } else {
        cout << "Invalid direction.\n";
        goto room6;
    }

room7:
    if (!hasKey6) {
        cout << "Room 7: You found a key for Room 6!\n";
        hasKey6 = true;
    }
    cout << "Go back North-West (NW)?\n";
    cin >> direction;
    if (direction == "NW") {
        goto room5;
    } else {
        cout << "Invalid direction.\n";
        goto room7;
    }

room8:
    if (!hasTorch) {
        cout << "Room 8: You found a torch!\n";
        hasTorch = true;
    }
    cout << "Go back North (N)?\n";
    cin >> direction;
    if (direction == "N") {
        goto room2;
    } else {
        cout << "Invalid direction.\n";
        goto room8;
    }
}
