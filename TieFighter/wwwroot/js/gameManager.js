// Note: Possibly create GameManager element that reflects the object.
// Reason: Add event listeners to the objects.

// #region Polyfilss

Array.prototype.last = function () {
    if (this.length == 0) {
        return null;
    } else {
        return this[this.length - 1];
    }
}

Array.prototype.first = function () {
    if (this.length == 0) {
        return null;
    } else {
        return this[0];
    }
}

var Helpers = {
    // Courtesy of http://forums.devshed.com/javascript-development-115/regexp-match-url-pattern-493764.html
    isUrl: function (str) {
        var pattern = new RegExp('^(https?:\/\/)?' + // protocol
            '((([a-z\d]([a-z\d-]*[a-z\d])*)\.)+[a-z]{2,}|' + // domain name
            '((\d{1,3}\.){3}\d{1,3}))' + // OR ip (v4) address
            '(\:\d+)?(\/[-a-z\d%_.~+]*)*' + // port and path
            '(\?[;&a-z\d%_.~+=-]*)?' + // query string
            '(\#[-a-z\d_]*)?$', 'i'); // fragment locater
        if (!pattern.test(str)) {
            return false;
        } else {
            return true;
        }
    }
}

// #endregion

class Submesh {
    constructor() {
        this.Id = window.location.href.split("/").last();
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

class Stage {
    constructor(stageConfig) {

    }
}

class Game {
    constructor(url) {
        function getDefinition(url) {
            
        }

        this._pathToGameDefinition = "js file/to load?";
        this._stages = [];
        this._gameAssets = {};
        this._gameDefinitions = {};
        this._gameFunctionDefinitions = {};

        XMLHttpRequestPromise("GET", url)
            .then(function (response) {

            }).catch(function (error) {

            });
    }
}

class GameManager {
    constructor(settings) {
        this._settings = null;
        this.missions = {};
        this.ui = {
            menus: {},
            canvas: settings.ui.canvas
        }
        this.loading = {};
        this.settings = settings;
        this._loadedBabylonAssets = [];
        this._gameStatus = GameManager.GameStatus.NOT_LOADED;
    }
    loadGame(game) {
        var getGameAssets = function () {
            return XMLHttpRequestPromise("GET", `${window.navigator.location.origin}/api/definition/${game}.json`)
                .then(function (response) {
                    var definition = JSON.parse(response);
                    var downloadPromises = [];
                    for (var href of definition.downloads) {
                        GameManager.dispatchEvent(GameManager.Events.loadingAsset(href.split("/").last()));
                        downloadPromises.push(XMLHttpRequestPromise("GET", href)
                            .then(function (download) {
                                var gameAsset = JSON.parse(download);
                                if (gameAsset.type == "BABYLON_ASSET") {
                                    GameManager._loadedBabylonAssets.push(gameAsset.toBabylon());
                                }
                                GameManager.dispatchEvent(GameManager.Events.finishedLoadingAsset(gameAsset.name));
                            }).catch(function (error) {
                                GameManager.dispatchEvent(GameManager.Events.errorLoading(error.assetName));
                            }));
                    }

                    Promise.all(downloadPromises)
                        .then(function () {

                        }).catch(function () {

                        })
                }).catch(function (error) {

                });
        }

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
    loadUserSettings(settings) {

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
    get addEventListener() {
        return this.ui.canvas.addEventListener;
    }
    get removeEventListener() {
        return this.ui.canvas.removeEventListener;
    }
    get dispatchEvent() {
        return this.ui.canvas.dispatchEvent;
    }
    static get Events () {
        return {
            loadingAsset: function (assetName) {
                return new CustomEvent("loadingAsset", { assetName: assetName });
            },
            finishedLoadingAsset: function (assetName) {
                return new CustomEvent("finishedLoadingAsset", { assetName: assetName });
            },
            errorLoadingAsset: function (assetName) {
                return new CustomEvent("errorLoadingAsset", { assetName: assetName });
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