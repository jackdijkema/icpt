#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>

int P1 = 1;   
int P2 = 2;   

typedef struct {
    int *board[7][6];  
} Board;

void makeMove(bool turn, Board *board);
void printBoard(Board *board);
void setupBoard(Board *board);
void updateBoard(Board *board, int move, bool turn);
void checkAndRemoveRows(Board *board, bool turn, int *p1, int *p2);

int main() {
    int p1 = 0, p2 = 0;
    bool turn = false;
    Board *board = malloc(sizeof(Board));

    setupBoard(board);

    while (1) {
        turn = !turn;
        printBoard(board);
        printf("Player 1 Points: %d | Player 2 Points: %d\n", p1, p2);
        makeMove(turn, board);
        checkAndRemoveRows(board, turn, &p1, &p2);
    }
}

void setupBoard(Board *board) {
    for (int col = 0; col < 7; col++) {
        for (int row = 0; row < 6; row++) {
            board->board[col][row] = NULL;  
        }
    }
}

void printBoard(Board *board) {
    printf("0123456\n");
    printf("-----------------\n");

    for (int row = 0; row < 6; row++) {
        for (int col = 0; col < 7; col++) {
            if (board->board[col][row] == NULL) {
                printf(".");
            } else if (*(board->board[col][row]) == 1) {
                printf("X"); 
            } else if (*(board->board[col][row]) == 2) {
                printf("+"); 
            }
        }
        printf("\n");
    }
    printf("-----------------\n");
}

void updateBoard(Board *board, int move, bool turn) {
    for (int row = 5; row >= 0; row--) {
        if (board->board[move][row] == NULL) {  // leeg veld
            board->board[move][row] = turn ? &P1 : &P2;
            return;
        }
    }
    printf("Column is full. Choose another column.\n");
}

void makeMove(bool turn, Board *board) {
    int input;
    char buffer[100];

    if (turn) {
        printf("Player 1 \n");
    } else {
        printf("Player 2 \n");
    }

    printf("Make a move: ");
    if (!fgets(buffer, sizeof(buffer), stdin)) {
        printf("Invalid input.\n");
        return;
    }

    if (sscanf(buffer, "%d", &input) != 1 || input < 0 || input > 6) {
        printf("Column out of range. Please choose between 0 and 6.\n");
        return;
    }

    updateBoard(board, input, turn);
}

void checkAndRemoveRows(Board *board, bool turn, int *p1, int *p2) {
    int target = turn ? 1 : 2;

    for (int row = 0; row < 6; row++) {
        int count = 0;
        for (int col = 0; col < 7; col++) {
            if (board->board[col][row] != NULL && *(board->board[col][row]) == target) {
                count++;
                if (count == 4) {
                    printf("Horizontal win detected!\n");
                    setupBoard(board); 
                    if (turn) (*p1)++;
                    else (*p2)++;
                    return;
                }
            } else {
                count = 0;
            }
        }
    }

    for (int col = 0; col < 7; col++) {
        int count = 0;
        for (int row = 0; row < 6; row++) {
            if (board->board[col][row] != NULL && *(board->board[col][row]) == target) {
                count++;
                if (count == 4) {
                    printf("Vertical win detected!\n");
                    setupBoard(board);
                    if (turn) (*p1)++;
                    else (*p2)++;
                    return;
                }
            } else {
                count = 0;
            }
        }
    }
}
