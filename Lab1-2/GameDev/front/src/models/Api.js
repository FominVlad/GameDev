const BASE_URL = 'https://localhost:44372';

export const URL = BASE_URL + '/api/Game';
export const START_GAME = 'PUT';
export const GET_MOVES = 'GET';
export const MAKE_MOVE = 'PATCH';

export const TYPES = {
    CPU: 0,
    PLAYER: 1,
};

export const COLORS = {
    BLACK: 0,
    WHITE: 1,
};