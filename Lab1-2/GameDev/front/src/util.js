export const createBoard = (size, chips = null) => {
    const board = new Array(size*size).fill(null).map((e, i) => ({x: i % size, y: Math.floor(i / size), type: 'none'}));
    if (chips) {
        return modifyBoard(board, chips);
    } else {
        return board;
    }
}

export const modifyBoard = (board, chips) => board.map(cell => {
    const chip = chips.find(chip => cell.x === chip.posX && cell.y === chip.posY);
    if (chip) {
        return Object.assign({}, cell, {ownerId: chip.ownerId});
    } else {
        return cell;
    }
});
