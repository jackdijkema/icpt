#include <stdio.h>
#include <stdbool.h>

typedef struct {
  char board[7][6];
} Board;

void makeMove(bool turn, Board *board);
void printBoard(Board *board);
void updateBoard(Board *board, int move, bool turn);
void checkAndRemoveRows(Board *board, bool turn, int *p1, int *p2);

int main() {
  int p1 = 0, p2 = 0;
  bool turn = false;
  Board board;

  for (int i = 0; i < 7; i++) {
    for (int j = 0; j < 6; j++) {
      board.board[i][j] = '0';
    }
  }

  while (1) {
    turn = !turn;
    printBoard(&board);
    printf("Player 1 Points: %d | Player 2 Points: %d\n", p1, p2);
    makeMove(turn, &board);
    checkAndRemoveRows(&board, turn, &p1, &p2);
  }
}

void printBoard(Board *board) {
  printf("0123456\n");
  printf("-----------------\n");

  for (int row = 0; row < 6; row++) {
    for (int column = 0; column < 7; column++) {
      printf("%c", board->board[column][row]);
    }
    printf("\n");
  }
  printf("-----------------\n");
}

void updateBoard(Board *board, int move, bool turn) {
  for (int row = 5; row >= 0; row--) {
    if (board->board[move][row] == '0') {
      board->board[move][row] = turn ? 'X' : '+';
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
  char symbol = turn ? 'X' : '+';
//horizontaal
  for (int row = 0; row < 6; row++) {
    int count = 0;
    for (int col = 0; col < 7; col++) {
      if (board->board[col][row] == symbol) {
        count++;
        if (count == 4) {
          printf("Horizontal win detected!\n");
          if (turn) (*p1)++;
          else (*p2)++;
          return;
        }
      } else {
        count = 0;
      }
    }
  }

//verticaal
  for (int col = 0; col < 7; col++) {
    int count = 0;
    for (int row = 0; row < 6; row++) {
      if (board->board[col][row] == symbol) {
        count++;
        if (count == 4) {
          printf("Vertical win detected!\n");
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
