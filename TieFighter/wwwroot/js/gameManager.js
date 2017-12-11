class Submesh {
    constructor() {

    }
    get initialScaling() {

    }
    get intialPosition() {

    }
    get intialRotation() {

    }
}

class ShipComponent {
    constructor(onUpdate) {

    }
    onUpdate(ship) {

    }
}

class BabylonAsset {
    constructor() {

    }
}

class Mission {
    constructor(id, name, number, briefing) {
        this._id = id;
        this._name = name;
        this._number = number;
        this._missionBriefing = briefing;
        this._babylonScene = {};
        this._babylonAssets = [];

        Object.freeze(this._id);
        Object.freeze(this._name);
        Object.freeze(this._number);
        Object.freeze(this._missionBriefing);
    }
    get id() {
        return this._id;
    }
    get name() {
        return this._name;
    }
    get missionBriefing() {
        return this._missionBriefing;
    }
}

class Tour {
    constructor(id, name, missions) {
        this._id = id;
        this._name = name;
        this._missions = missions;

        Object.freeze(this._id);
        Object.freeze(this._name);
        Object.freeze(this._missions);
    }
    get id() {
        return this._id;
    }
    get name() {
        return this._name;
    }
    get missions() {
        return this._missions;
    }
}

class VideoSettings {
    constructor() {
        this.resulotion = {};
        this.modelQuality = {};
        this.viewDistance = 100;
    }
}

class AudioSettings {
    constructor(masterVolume, musicVolume, combatVolume) {
        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.combatVolume = combatVolume;
    }
}

class ControlSettings {
    constructor(json) {
        this.forward = [];
        this.left = [];
        this.reverse = [];
        this.right = [];
        this.pitch = [];
        this.yaw = [];
        this.roll = [];
        this.primaryFire = [];
        this.secondaryFire = [];
        this.switchWeapons = [];
        if (json === null) {
            // Use default settings
            this.forward = ["ArrowUp"];
            this.left = ["ArrowLeft"];
            this.reverse = ["ArrowDown"];
            this.right = ["ArrowRight"];
            this.pitch = [];
            this.yaw = [];
            this.roll = [];
            this.primaryFire = [0];
            this.secondaryFire = [2];
            this.switchWeapons = [1];
        } else {
            // Parse json and set settings
            this.left = [];
            this.reverse = [];
            this.right = [];
            this.pitch = [];
            this.yaw = [];
            this.roll = [];
            this.primaryFire = [];
            this.secondaryFire = [];
            this.switchWeapons = [];
        }
    }
}

class UserSettings {
    constructor(audioSettings, controlSettings, videoSettings) {
        this._audio = null;
        this._control = null;
        this._video = null;
        this.audio = audioSettings;
        this.control;
        this.video;
    }
    get control() {
        return this._control;
    }
    set control(value) {
        if (!(value instanceof ControlSettings)) {
            throw new Error("The argument 'value' must be an instance of ControlSettings");
        }

        this._control = value;
    }
    get audio() {
        return this._audio;
    }
    set audio(value) {
        if (!(value instanceof AudioSettings)) {
            throw new Error("The argument 'value' must be an instance of AudioSettings");
        }

        this.audio = value;
    }
    get video() {
        return this._video;
    }
    set video(value) {
        if (!(value instanceof VideoSettings)) {
            throw new Error("The argument 'value' must be an instance of VideoSettings");
        }

        this._video = value;
    }
}

class Game {
    constructor() {
        this._gameDefinitions = "js file/to load?";
        this._stages = [];
    }
}

class GameManager {
    constructor(settings) {
        this._settings = null;

        this.missions = {};
        this.menus = {};
        this.loading = {};
        this.settings = settings;
        this._loadedBabylonAssets = [];
        this._gameStatus = GameManager.GameStatus.NOT_LOADED;
    }
    loadGame(game) {
        if (!(game instanceof Game)) {
            throw new Error("The argument 'game' must be an instance of Game.");
        }

        this._gameStatus = GameManager.GameStatus.LOADING;
    }
    pauseGame() {
        this._gameStatus = GameManager.GameStatus.PAUSED;
    }
    exitGame() {
        this._gameStatus = GameManager.GameStatus.UNLOADING;
    }
    get settings() {
        return this._settings;
    }
    set settings(value) {
        if (!(value instanceof UserSettings)) {
            throw new Error("The argument 'value' must be an instance of UserSettings");
        }

        this._settings = value;
    }
    get ui() {
        return {
            menus: {
                mainMenu,
                inGameMenu
            },
            loadingMenus
        }
    }
    loadUserSettings(settings) {

    }
    static get Events () {
        return {
            loadingShip: function (shipName) {
                return new CustomEvent("loadingShip", { shipName: shipName });
            },
            finishedLoadingShip: function (shipName) {
                return new CustomEvent("finishedLoadingShip", { shipName: shipName });
            },
            errorLoadingShip: function (shipName) {
                return new CustomEvent("errorLoadingShip", { shipName: shipName });
            },
            loadingScene: function (sceneName) {
                return new CustomEvent("sceneLoading", { sceneName: sceneName });
            },
            finishedLoadingScene: function (sceneName) {
                return new CustomEvent("finishedLoadingScene", { sceneName: sceneName })
            },
            errorLoadingScene: function (sceneName) {
                return new CustomEvent("errorLoadingScene", { sceneName: sceneName });
            },
            updateControls: function () {
                return new CustomEvent("controlsWereUpdated");
            }
        }
    }
    static get GameStatus() {
        return {
            NOT_LOADED: 0,
            LOADING: 1,
            RUNNING: 2,
            PAUSED: 3,
            UNLOADING: 4,
            ERROR: 5
        }
    }
}