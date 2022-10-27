<p align="center"><img
  src="https://www.wallenart.dev/src/images/plankton.svg"
  alt="nouvell logo" /></p>
 
 <p align="center">
<img alt="GitHub code size in bytes" src="https://img.shields.io/github/languages/code-size/zyriabdsgn/plankton-synapse">
<img alt="GitHub" src="https://img.shields.io/github/license/zyriabdsgn/plankton-synapse">
</p>

A turn-based strategy board game based on planktons and cellular reproduction.

Original concept, art and game design: Arthur Vasović  
Programming, software design and UX: Arthur Wallendorff

Made with Unity, game logic mostly developed on a Sony Xperia e4g 🔥

![gif of the game](https://github.com/ZyriabDsgn/Plankton-Synapse/blob/ed25c60166f00c37a43a6f758ac4b83de39d70a1/sprites/demo.gif)

# Installation

In order to install/compile the game you need to install Unity and compile it from there.

# Usage

You can find the latest build by [following this link](https://zyriabdsgn.github.io/Plankton-Synapse/).

**Only local 1v1 PvP is implemented**, you can read the rules below to understand how everything works.  
Otherwise, the game highlights the possible moves.

# Game rules
<details>
<summary>Click to unwrap</summary>

## The board

Just like chess, Plankton Synapse is played on a board of 64\*64 squares with fixed starting positions for all pieces. Although at its core the game is completely different from chess, there still are many rules that both share; if something is left unspecified in the rules, you can safely assume that the equivalent rule from chess is used in Plankton Synapse.

## Cells and phagocytosis

Pieces are called cells; if a cell is alive, it is on the board, and killing it removes it from the board for the remainder of the game; if a move from a cell of the opponent is responsible for this loss, then this event is called a phagocytosis (corresponding to capture in chess); usually, a cell which moves onto a square occupied by an opponent’s cell will phagocytize that cell. Although it is possible for one’s own cells to commit suicide through apoptosis, it is not possible for one cell to phagocitize another friendly cell. There are nine different types of cell in Plankton Synapse, and each has different rules which are explained in the Info tab whenever the corresponding cell is selected. Two of them – the picozoa and the picophyte – are not on the board when the game starts and to be used must first be generated by neatoms.

## Turn order

Just like chess, Plankton Synapse is a turn-based game. Turns are played sequentially: only one player can take action during a given turn. The first player to move is blue, and the second one is grey. During a given turn, exactly one cell, which must be controlled by the player to move and is chosen by them, must be selected to take action, which is called a move. Plankton Synapse distinguishes between normal turns and free turns. Free turns only occur when triggered by specific moves and are considered to be a part of the normal turn during which they were triggered.

## Apoptosis and free turns

Instead of playing a normal move, it is always possible to destroy one of your own cells, unless said cell is a zoorb – the rough equivalent to the king in chess. This move is called an apoptosis. However, it is never possible to do nothing when it is your turn to move. Only free turns – that is to say those triggered by an empowered neatom, an empowered tetraphyte or an empowered pentaphyte after they move normally – can be ignored, and they are therefore considered facultative. Such free turns also have restrictions that prevent them from being used to commit apoptosis.

## Stalemate and victory

Stalemates are extremely rare and only occur whenever a given position of the entire board occurs for the third time in the entire game OR if one player has no authorized move during one of their non-free turns; but having no authorized move is unlikely as it is always possible to commit apoptosis with a non-zoorb cell. The goal of the game is to destroy all zoorbs controlled by the opponent; if at any point during the game a player has no zoorb on the board, then the game ends immediately and that player loses. However, unlike in chess, it is also allowed to play moves that would put one’s zoorbs in immediate danger.

## Empowerment

All cells except for zoorbs may exist in two distinct states:normal state and empowered state. An empowered cell unlocksadditional effects for itself. By default, cells always are in anormal state; a given cell is considered empowered for the entiretyof a given turn if and only if at the beginning of this turn it isaffected by an odd number of effects inverting its power state.Such effects can be stacked infinitely and are produced only by theexistence of friendly zoorbs and by alignement with phytorbs. Also,free turns do not reset the power state of cells; a given cellalways keeps the same power state during a free turn as during thenormal turn that triggered it.

## Moves

A cell can either move by itself or be moved by another cell. These two types of movement matter because of some effects associated with picophytes and picozoon. A given move cannot pass through a square that is occupied by another cell unless the opposite is specified in the move’s description (jumps from pentazoon and pentaphytes) or the square is occupied by a cell of the opponent and is determined as the move’s destination (phagocytosis). A given movement can result in a phagocytosis only if its author moved by itself. Some moves are not allowed to end in a square occupied by the enemy, and thus are either entirely unable to perform phagocytosis or do so under different conditions.

## Pieces

### Neatom

![Neatom](https://github.com/ZyriabDsgn/Plankton-Synapse/blob/2d9b0e0016d94ae4e94db0e3275a7049e0c3df64/sprites/Globule%20A1glow.png?raw=true)

The neatom is the base cell in Plankton Synapse, roughly equivalent to the pawn in chess. However, it has very different mechanics as it is incapable of moving by itself or phagocitizing other pieces directly. The Neatom’s possible moves are the following:

**Duplication**: generates a new friendly neatom on an adjacent unoccupied square in any direction.

**Minor fusion**: this move can only be performed if another friendly neatom is located in an orthogonally adjacent square. The neatom that performs this move as well as one other friendly neatom located in an orthogonally adjacent square are both killed, and the neatom that performed this move is replaced by a new friendly picozoa or a new friendly picophyte as chosen by the player.

**Major fusion**: this move can only be performed if the neatom forms an orthogonal square with three adjacent friendly neatoms. The neatom that performs this move as well as three other adjacent friendly neatoms forming an orthogonal square with it are all killed, and the neatom that performed this move is replaced by a new friendly cell of any type chosen by the player except for neatom, picozoa or picophyte. If the selected cell type is tetraphyte, then the created cell occupies the same square that the sacrificed neatoms previously formed together.

When empowered, a neatom also gains the following effects: after this cell performs a normal (non-free) move, a friendly neatom (which may or may not be the same) may immediately perform a free move.

### Picophyte

![Picophyte](https://github.com/ZyriabDsgn/Plankton-Synapse/blob/2d9b0e0016d94ae4e94db0e3275a7049e0c3df64/sprites/Triglobe%20A1glow.png?raw=true)

The picophyte is special in that, like the picozoa, it doesn’t start on the board and must be generated by neatoms before being used. Like picozoon, it is similar to the pawn in chess in that it has very limited movement but can phagocitize enemy cells. The Picophyte’s possible moves are the following:

**Movement**: moves orthogonally by 1 square. When the picophyte's movement is complete, all orthogonally adjacent cells that may be moved are pushed back by 1 square in the direction opposite to the picophyte, unless this would move them to an occupied square.

In addition, the picophyte passively benefits from the following effects at all times: if a cell passes through a free square that is orthogonally adjacent to the picophyte, it is forced to stop its movement on that square, unless it was already there before that movement was initiated. Both allied and enemy cells are affected, and they are affected regardless of if they moved by themselves of were moved by another cell. This effect may also interrupt a phytorb’s movement before it is complete.

When empowered, a picophyte also gains the following effects: the picophyte's movement range becomes unlimited, but the picophyte cannot phagocitize another cell when moving by more than 1 square.

### Picozoa

![Picozoa](https://github.com/ZyriabDsgn/Plankton-Synapse/blob/2d9b0e0016d94ae4e94db0e3275a7049e0c3df64/sprites/Triastre%20A1glow.png?raw=true)

The picozoa is special in that, like the picophyte, it doesn’t start on the board and must be generated by neatoms before being used. Like picophytes, it is similar to the pawn in chess in that it has very limited movement but can phagocitize enemy cells.

**Movement**: moves vertically by 1 or 2 squares. When the picozoa's movement is complete, all cells that may be moved and are located at range 2 orthogonally are attracted towards the picozoa, unless this would move them to an occupied square.

In addition, the picozoa passively benefits from the following effects at all times: when a picozoa is phagocitized, the cell that phagocitized it is destroyed and replaced by a new neatom under the same player’s control.

When empowered, a picozoa benefits from the following effects at all times: cells that are orthogonally adjacent to the picozoa may not be moved by other cells, and enemy neatoms cannot be generated in squares that are orthogonally adjacent to the picozoa.

### Tetraphyte

![Tetraphyte](https://github.com/ZyriabDsgn/Plankton-Synapse/blob/2d9b0e0016d94ae4e94db0e3275a7049e0c3df64/sprites/Tetraglobe%20A1glow.png?raw=true)

The tetraphyte may be the most recognizable cell in the game, because it is massive enough to single-handedly occupy four squares at all times rather than one like the other cells. It is also unique in that each player starts the game with only one tetraphyte, rather two or none like the other cells.

**Movement**: moves by 1 square in any direction.

In addition, the tetraphyte passively benefits from the following effects at all times: constantly occupies four squares by being placed at their intersection, and may phagocitize several cells in a single move.

When empowered, a tetraphyte benefits from the following effects: after this cell performs a normal (non-free) move, it may immediately perform a free move.

### Tetrazoa

![Tetrazoa](https://github.com/ZyriabDsgn/Plankton-Synapse/blob/2d9b0e0016d94ae4e94db0e3275a7049e0c3df64/sprites/Tetrastre%20A1glow.png?raw=true)

Although the tetrazoa is generally unable to phagocitize other cells, it also is itself generally immune to phagocytosis. As such, it may be used as an indestructible obstacle to shape the board according to its controller’s will.

**Movement**: moves by 1 or 2 squares orthogonally or by 1 square diagonally. This movement cannot end on an occupied square and therefore cannot be used to phagocitize enemy cells.

In addition, the tetrazoa passively benefits from the following effects at all times: only empowered tetrazoon may phagocitize other tetrazoon. Tetrazoon may not be displaced by other cells except for pentaphytes.

When empowered, a tetrazoa benefits from the following effects: this cell’s movement range is unlimited and it can phagocitize other tetrazoon, thus allowing it to end in occupied squares in some cases.

### Pentaphyte

![Pentaphyte](https://github.com/ZyriabDsgn/Plankton-Synapse/blob/2d9b0e0016d94ae4e94db0e3275a7049e0c3df64/sprites/Pentaglobe%20A1glow.png?raw=true)

Although the pentaphyte is entirely incapable of doing harm on its own, it still is very useful as it provides powerful movement options to its allies.

**Interversion**: swaps its position with any friendly cell that isn’t a pentaphyte. If the targetted cell is a tetraphyte, the pentaphyte arrives in any of the squares that the tetraphyte previously occupied, and the tetraphyte now occupies any entirely empty square of 4 within which the pentaphyte used to be located; a tetraphyte can only be targetted by this move if there is such a square for it to arrive. The targetted cell is considered as being moved by the pentaphyte.

When empowered, a pentaphyte benefits from the following effects: after having interverted its position with a neatom, the pentaphyte may kill this neatom to immediately perform a free move.

### Pentazoa

![Pentazoa](https://github.com/ZyriabDsgn/Plankton-Synapse/blob/2d9b0e0016d94ae4e94db0e3275a7049e0c3df64/sprites/Pentastre%20A1glow.png?raw=true)

The pentazoa is an interesting piece because, although it may move in any direction and through any distance like a queen in chess, it may also jump over other cells and even over empty squares like a knight in chess. However its movement has other strong restrictions which make it much less powerful than the simple fusion of a queen and a knight would be.

**Movement**: moves in any direction by any even number of squares in a straight line. May jump over a single square at any point during this movement (which allows it to avoid passing through a square as well as to pass through an obstacle), after which this movement stops immediately. If the pentazoa jumped over an enemy cell that may be phagocitized, that enemy cell is phagocitized. The pentazoa cannot end its movement on a square that is already occupied, even by the enemy.

When empowered, a pentazoa benefits from the following effects: an empowerment pentazoa may jump over any number of squares during its movement provided that the square located right after the one jumped over is free; thus, jumping over a square no longer ends the pentazoa’s movement when it is empowered. All enemy cells that are jumped over during one such movement are phagocitized, thus allowing empowered pentazoon to phagocitize several cells per move. An empowered pentazoa may also move an odd number of squares if so desired; however, it remains unable to change direction at any point during its movement.

### Phytorb

![Phytorb](https://github.com/ZyriabDsgn/Plankton-Synapse/blob/2d9b0e0016d94ae4e94db0e3275a7049e0c3df64/sprites/Rosace%20A1glow.png?raw=true)

Although it is entirely unable to perform phagocitosis, the phytorb is an extremely important cell because only it allows players to directly control the power state of other cells, both to empower allies and to deny empowerment to enemies.

**Movement**: moves diagonally as far as possible until reaching an obstacle, and then moves by 1 square in any direction. This movement cannot end on an occupied square and therefore cannot be used to phagocitize enemy cells.

In addition, the phytorb passively benefit from the following effect at all times: at the beginning of a normal (non-free) turn, if a cell is located on the same column, row or diagonal as a phytorb without being separated from it by any obstacle, it receives an additional power state inversion until the next normal (non-free) turn begins. This effect stacks for each adequately placed phytorb. If the affected cell is a zoorb, instead of receiving an additional power state inversion, it cannot be phagocitized. Both enemy and friendly cells are affected by this ability.

When empowered, a phytorb benefits from the following effects: all non-phytorb cells located in the same column, row or diagonal as this cell are now affected by its passive ability, even if obstacles separate them.

### Zoorb

![Zoorb](https://github.com/ZyriabDsgn/Plankton-Synapse/blob/2d9b0e0016d94ae4e94db0e3275a7049e0c3df64/sprites/Astree%20A1glow.png?raw=true)

The zoorb is the most important cell in the game for more than one reason. First, as the equivalent of the king in chess, it determines who wins or loses the game. But in addition to that, by its mere existence it also alters the power state of all friendly cells on the board. It should also be noted that each player starts the game with two zoorbs and is perfectly allowed to generate even more through neatoms.

**Movement**: jumps over an occupied adjacent square in any direction. This movement may only be used if there is an adjacent cell to be jumped over, and it can as normal be used to phagocitize a cell that occupies its ending square.

**Biogenesis**: generates a new friendly neatom on a free adjacent square in any direction.

In addition, the zoorb passively benefits from the following effects at all times: at the beginning of a normal (non-free) turn, if this cell is on board, the power state of all friendly cells is inverted until the next normal (non-free) turn begins. This effect stacks for each friendly zoorb on the board.

If at any point during the game a player has no zoorb on the board, then the game ends immediately and that player loses.

</details>
