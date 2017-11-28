$('[data-toggle="tooltip"]').tooltip();
$("#ExitGame").on("click", function () {
    // FIXME: Replace this function
    window.location.assign(window.location.origin);
});

$("#returnToMainMenu").on("click", function () {
    window.location.assign(window.location.origin + "/Game/TieFighter");
});

var mainDisplay = $("#mainDisplay")
$("#shipViewPort #mainDisplay input.toggleDisplay").on("change", function () {
    if (this.checked) {
        mainDisplay.addClass("open");
    } else {
        mainDisplay.removeClass("open");
    }
});

$("#resumeGame").on("click", resumeGame);

function loadingScreen(show) {
    show = Boolean(show);
    document.getElementById("mainLoading").style.display = show ? "block" : "none";
}

class WeaponDefinition {
    constructor(name, dmg, dmgRadius, dmgFalloff = null) {

    }
    get name() {
        return this._name;
    }
    get dmg() {
        return this._dmg;
    }
    get dmgRadius() {
        return this._dmgRadius;
    }
    get dmgFalloff() {
        return this._dmgFalloff;
    }
}

class EngineDefinition {
    constructor(name, fuelConsumedPerSecond) {
        this._name = name;
        this._fuelConsumedPerSecond = Number(fuelConsumedPerSecond);
    }
    get name() {
        return this._name;
    }
    get fuelConsumedPerSecond() {

    }
    calculateFuelConsumed(seconds) {
        return this.fuelConsumedPerSecond * seconds;
    }
}

class EventListenerWrapper {
    constructor(element, on, func) {
        this._element = element;
        this._on = on;
        this._func = func;
        this._isDestroyed = false;
        element.addEventListener(on, func);
    }
    get isDestroyed () {
        return this._isDestroyed;
    }
    destroy () {
        if (!this.isDestroyed) {
            this._element.removeEventListener(on, func);
            this._isDestroyed = true;
        }
    }
}

class Mission {
    constructor (name, number, briefing) {
        this._name = name;
        this._number = number;
        this._missionBriefing = briefing;

        Object.freeze(this._name);
        Object.freeze(this._number);
        Object.freeze(this._missionBriefing);
    }
    get name () {
        return this._name;
    }
    get missionBriefing () {
        return this._missionBriefing;
    }
}

class Tour {
    constructor (name, missions) {
        this._name = name;
        this._missions = missions;
        Object.freeze(this._name);
        Object.freeze(this._missions);
    }
    get name () {
        return this._name;
    }
    get missions () {
        return this._missions;
    }
}

class Accelerations {
    constructor(translationVector, rotationVector) {
        if (!(translationVector instanceof BABYLON.Vector3) ||
            !(rotationVector instanceof BABYLON.Vector3))
        {
            throw new Error("The arguments 'translationVector' and 'rotationVector' must be instances of BABYLON.Vector3");
        }

        this._x = translationVector.x;
        this._y = translationVector.y;
        this._z = translationVector.z;
        this._pitch = rotationVector.x;
        this._yaw = rotationVector.y;
        this._roll = rotationVector.z;
    }
    get x() {
        return this._x;
    }
    get y() {
        return this._y;
    }
    get z() {
        return this._z;
    }
    get pitch() {
        return this._pitch;
    }
    get yaw() {
        return this._yaw;
    }
    get roll() {
        return this.roll;
    }
}

class Power {
    constructor(maxFuel, currentFuel = null) {
        this._maxFuel = Number(maxFuel);
        if (currentFuel == null) {
            this._currentFuel = this.maxFuel;
        } else {
            this._currentFuel = Number(currentFuel);
        }
    }
    get maxFuel() {
        return this._maxFuel;
    }
    get currentFuel() {
        return this._currentFuel;
    }
}

class Ship {
    constructor(filename, accelerations, meshes, power) {
        if (arguments.length == 1) {

            // Argument is json
            var parsedJson = JSON.parse(arguments[0]);
            filename = parsedJson.filename;
            accelerations = parsedJson.accelerations;
            meshes = parsedJson.meshes;
            power = parsedJson.power;
        }

        // Validate

        if (filename == null || filename == "") {
            throw new Error("The argument 'filename' cannot be null nor empty.")
        }
        else if (accelerations == null ||
            !(accelerations instanceof Accelerations)) {
            throw new Error("The argument 'accelerations' cannot be null "
                + "and must be an instance of Accelerations.");
        }
        else if (meshes == null || !(meshes instanceof Array)) {
            throw new Error("The argument 'meshes' cannot be null and must "
                + "be an Array.");
        }
        else if (power == null || !(power instanceof Power)) {
            throw new Error("The argument 'power' cannot be null and must "
                + "be an instance of Power");
        }

        this.modalFileName = filename;
        this.modalName = filename.replace(".babylon", "");

        Object.freeze(this.modalFileName);
        Object.freeze(this.modalName);

        throw Game.events.shipLoading(this.modalName);

        this.accelerations = accelerations;
        this.meshes = meshes;
        this.power = power;

        // Import mesh
        loadMeshModal(this.modalFileName)
            .then(function(mesh) {
                console.log("Loaded mesh ok.");
                this.BABYLON_mesh = mesh;
                throw Game.events.finishedShipLoading(this.modalName);
            }).catch(function (error) {
                throw Game.events.errorLoadingShip(this.modalName);
            });
    }
}

var Game = {
    events: {
        shipLoading: function (shipName) {
            return new CustomEvent("shipLoading", { shipName: shipName });
        },
        finishedShipLoading: function (shipName) {
            return new CustomEvent("finishedShipLoading", { shipName: shipName });
        },
        errorLoadingShip: function (shipName) {
            return new CustomEvent("errorLoadingShip", { shipName: shipName });
        }
    },
    menus: {
        mainMenu: {
            init (tours) {
                for (var tour of tours) {

                    // Check if the variable is an instance of Tour
                    if (!(tour instanceof Tour)) {
                        continue; // Not an instance
                    } else {
                        for (var mission of tour.missions) {
                            var missionButton = document.createElement("button");
                        }
                    }
                }
            },
            show () {
                $("#mainMenu").show();
            },
            hide () {
                $("#mainMenu").hide();
            }
        },
        inGameMenu: {
            show () {
                $("#inGameMenu").show();
            },
            hide () {
                $("#inGameMenu").hide();
            }
        },
        settings: {
            pointOfView: {
                options: [
                    "first person",
                    "third person"
                ],
                indexOfActiveOption: 0
            },
            flightControls: {
                forwards: {

                },
                reverse: {

                },
                left: {

                },
                right: {

                },
                pitch: {

                },
                yaw: {

                },
                roll: {

                },
                fire: {

                }
            }
        }
    },
    userSettings: {
        resolution: ""
    }
}

var loadedMeshes = [];

var canvas = {};
var engine = {};
var scene = {};
var camera = {};
var cameras = [];
var tiefighter = {};
var stardestroyer = {};
window.addEventListener("DOMContentLoaded", function () {
    // Show menu
    var tours = [];
    XMLHttpRequestPromise("GET", "/tours.json")
        .then(function (response) {
            var json = JSON.parse(response).tours;
            for (var t of json) {
                var missions = [];
                for (var m of t.missions) {
                    missions.push(new Mission(m.name, m.number, m.missionBriefing));
                }

                tours.push(new Tour(t.name, missions));
            }

            var missionListEl = $("#missions");
            for (var tour of tours) {
                var container = document.createElement("div");
                var title = document.createElement("div");
                var ul = document.createElement("ul")

                title.innerText = tour.name;
                for (var mission of tour.missions) {
                    var liEl = document.createElement("li");
                    liEl.innerText = mission.name;
                    ul.appendChild(liEl)
                }

                container.appendChild(title);
                container.appendChild(ul);
                missionListEl.append(container);
            }
        }).catch(function (error) {
            console.error("Failed to retrieve tours!");
        });
    $("#tutorial").on("click", function () {
        loadingScreen(true);
        Game.menus.mainMenu.hide();
        loadGame();
        loadingScreen(false);
    });
    Game.menus.mainMenu.show();
    loadingScreen(false);
});

function loadGame () {
    document.getElementById("shipViewPort").style.display = "block";
    canvas = document.getElementById("scene");
    engine = new BABYLON.Engine(canvas, true);
    var skybox = {}
    
    // var loader = new BABYLON.AssetsManager(canvas);
    // var tiefighter = loader.addMeshTask("tiefighter", "", "/public", "tiefighter.stl")

    var createScene = function () {
        // var scene = new BABYLON.Scene(engine);
        // var camera = new BABYLON.FreeCamera("MainCamera", new BABYLON.Vector3(0,5,-10));

        scene = new BABYLON.Scene(engine);
        var gravityVector = new BABYLON.Vector3(0, 0, 0);
        scene.enablePhysics(gravityVector);
        var light = new BABYLON.PointLight("Omni", new BABYLON.Vector3(0, 100, 100), scene);
        camera = new BABYLON.ArcRotateCamera("Camera", 0, 0.8, 100, BABYLON.Vector3.Zero(), scene);
        //camera = new BABYLON.UniversalCamera("Camera", BABYLON.Vector3.Zero(), scene);
        camera.attachControl(canvas, false);
        camera.keysDown = [];
        camera.keysUp = [];
        camera.keysLeft = [];
        camera.keysRight = [];
        scene.clearColor = BABYLON.Color3.Black();
        //skybox = BABYLON.Mesh.CreateBox("skybox", 100, scene);
        //var skyboxMaterial = new BABYLON.StandardMaterial("skybox", scene);
        //skyboxMaterial.backFaceCulling = false;
        //skyboxMaterial.reflectionTexture = new BABYLON.CubeTexture("/textures/backgroundB", scene, ["_px.png","_py.png","_pz.png","_nx.png","_ny.png","_pz.png"]);
        //skyboxMaterial.reflectionTexture.coordinatesMode = BABYLON.Texture.SKYBOX_MODE;
        //skyboxMaterial.diffuseColor = new BABYLON.Color3(0, 0, 0);
        //skyboxMaterial.specularColor = new BABYLON.Color3(0, 0, 0);
        //skyboxMaterial.reflectionTexture.hasAlpha = true;
        //skyboxMaterial.disableLighting = true;
        //skybox.material = skyboxMaterial;
        //skybox.parent = camera;
        var light = new BABYLON.HemisphericLight("light1", new BABYLON.Vector3(0,1,0), scene);
        // var sphere = BABYLON.Mesh.CreateSphere("sphere1", 16, 2, scene);
        // sphere.position.y = 1;
        // var cube = BABYLON.Mesh.CreateBox("box", 2.0, scene, false, BABYLON.Mesh.DEFAULTSIDE);
        // cube.position.y = 1;
        // cube.position.x = 3;

        // var sphere1 = BABYLON.Mesh.CreateSphere("Sphere1", 10.0, 6.0, scene);
        // var sphere2 = BABYLON.Mesh.CreateSphere("Sphere2", 2.0, 7.0, scene);
        // var sphere3 = BABYLON.Mesh.CreateSphere("Sphere3", 10.0, 8.0, scene);
        // var sphere4 = BABYLON.Mesh.CreateSphere("Sphere4", 10.0, 8.0, scene);
        // var sphere5 = BABYLON.Mesh.CreateSphere("Sphere5", 10.0, 8.0, scene);
        // var sphere6 = BABYLON.Mesh.CreateSphere("Sphere6", 10.0, 8.0, scene);

        // sphere1.position.x = -40;
        // sphere2.position.x = -30;
        // sphere3.position.x = -5;
        // sphere4.position.x = 5;
        // sphere5.position.x = 30;
        // sphere6.position.x = 40;

        // var material1 = new BABYLON.StandardMaterial("texture1", scene);
        // material1.alpha = 0.5;
        // material1.diffuseColor = new BABYLON.Color3(1, 0.2, 0.7);
        // material1.emissiveColor = new BABYLON.Color3(1, .2, .7);
        // material1.ambientColor = new BABYLON.Color3(1,0.2,0.7);
        // material1.specularColor = new BABYLON.Color3(1, 0.2, 0.7);
        // material1.specularPower = 32;

        // sphere1.material = material1;
        // // sphere2.material = new BABYLON

        // var ground = BABYLON.Mesh.CreateGround("ground1", 100, 100, 0, scene);

        // var animationBox = new BABYLON.Animation("myAnimation", "scaling.x", 30, BABYLON.Animation.ANIMATIONTYPE_FLOAT, BABYLON.Animation.ANIMATIONLOOPMODE_CYCLE);
        // var keys = [];
        // keys.push({
        //     frame: 0,
        //     value: 1
        // });

        // keys.push({
        //     frame: 20,
        //     value: 0.2
        // });

        // keys.push({
        //     frame: 100,
        //     value: 1
        // });

        // animationBox.setKeys(keys);
        // sphere1.animations = [];
        // sphere1.animations.push(animationBox);

        // scene.beginAnimation(sphere1, 0, 100, true);

        BABYLON.SceneLoader.ImportMesh("", "/models/", "tiefighter.babylon", scene, function (newMesh) {
            // camera.setTarget(newMesh);

            tiefighter = BABYLON.MeshBuilder.CreateBox("tiefighterParent", {}, scene);
            tiefighter.visibility = 0;
            //tiefighter.scaling.x = 5;
            //tiefighter.scaling.y = 5;
            //tiefighter.scaling.z = 5;
            for (var i = 0  ; i < newMesh.length; i++) {
                newMesh[i].parent = tiefighter;
                //newMesh[i].scaling.x = -5;
                //newMesh[i].scaling.y = -5;
                //newMesh[i].scaling.z = -5;
            }

            tiefighter.rotation.y = Math.PI;

            //camera.setTarget(tiefighter, new BABYLON.Vector3.Zero());
            //camera.attachControl(tiefighter);
            //scene.actionManager.registerAction(new BABYLON.ExecuteCodeAction(BABYLON.ActionManager.OnKeyDownTrigger, function (evt) {
            //    map[evt.sourceEvent.key] = evt.sourceEvent.type == "keydown";
        
            //}));

            //camera = new BABYLON.ArcFollowCamera("Camera", 0, 0.8, 100, new BABYLON.Vector3.Zero(), scene);

            //camera.setTarget(BABYLON.Vector3.Zero(), true, true);
            //camera.attachControl(canvas, false);

            //tiefighter.parent = camera;
            camera.parent = tiefighter;
            camera.setPosition(new BABYLON.Vector3(-0.3, 6, 90));
            camera.targetScreenOffset.y = -10
            //camera.position.x = -0.30276579156741895;
            //camera.position.y = 5.743716525255501;
            //camera.position.z = 99.83445323811294;
            //camera.lockTarget = tiefighter;
            tiefighter.position.x = 0;
            tiefighter.position.y = -20;
            tiefighter.position.z = -500;
            //tiefighter.addRotation(Math.PI, 0, Math.PI);
            tiefighter.physicsImpostor = new BABYLON.PhysicsImpostor(tiefighter, BABYLON.PhysicsImpostor.BoxImpostor, { mass: 1, restitution: 0.9, disableBidirectionalTransformation: true }, scene);
            //camera.rotation.y = Math.PI;

            // Scroll evt listener
            window.addEventListener("scroll", function (evt) {
                console.log(evt);
            });

            // Scene starts
            engine.runRenderLoop(function () {
                scene.render();
            });
        }, null, function (error) {
            throw error;
        });

        BABYLON.SceneLoader.ImportMesh("", "/models/", "stardestroyermark1.babylon", scene, function (newMesh) {
            stardestroyer = newMesh[0];
            for (var i = 1; i < newMesh.length; i++) {
                newMesh[i].parent = stardestroyer;
            }

            stardestroyer.rotation.y = Math.PI;

            stardestroyer.scaling.x = 100;
            stardestroyer.scaling.y = 100;
            stardestroyer.scaling.z = 100;

            stardestroyer.position.z = 1105;

            stardestroyer.physicsImpostor = new BABYLON.PhysicsImpostor(stardestroyer, BABYLON.PhysicsImpostor.BoxImpostor, { mass: 0, restitution: 0.9, disableBidirectionalTransformation: true }, scene);
        })

        return scene;
    }

    scene = createScene();
    
    // object for multiple key presses
    var map = {};
    scene.actionManager = new BABYLON.ActionManager(scene);
    scene.actionManager.registerAction(new BABYLON.ExecuteCodeAction(BABYLON.ActionManager.OnKeyDownTrigger, function (evt) {
        map[evt.sourceEvent.key] = true;
    }));
    scene.actionManager.registerAction(new BABYLON.ExecuteCodeAction(BABYLON.ActionManager.OnKeyUpTrigger, function (evt) {
        delete map[evt.sourceEvent.key];
    }));
    scene.registerAfterRender(function (test) {

        var elapsed = engine.getDeltaTime() / 1000;

        var zRot = 0;
        var xRot = 0;
        var yRot = 0;

        // Update transforms
        if ((map["w"] || map["W"])) {
            //camera.rotation.x += Math.PI / 360;
            //tiefighter.physicsImpostor.applyImpulse(new BABYLON.Vector3(10, 10, 0), tiefighter.getAbsolutePosition());
            //tiefighter.rotatePOV(Math.PI / 360, 0, 0);
            //xRot += elapsed * turnSpeed;
            xRot += elapsed * 2;
        };

        if ((map["s"] || map["S"])) {
            //camera.rotation.x -= Math.PI / 360;
            //tiefighter.rotatePOV(-Math.PI / 360, 0, 0);
            //xRot -= elapsed * turnSpeed;
            xRot -= elapsed * 2;
        };

        if ((map["d"] || map["D"])) {
            //camera.rotation.y += Math.PI / 360;
            //tiefighter.rotatePOV(0, 0, -Math.PI / 360);
            //yRot += elapsed * turnSpeed;
            yRot += elapsed * 2;
        };

        if ((map["a"] || map["A"])) {
            //camera.rotation.y -= Math.PI / 360;
            //tiefighter.rotatePOV(0, 0, Math.PI / 360);
            //yRot -= elapsed * turnSpeed;
            yRot -= elapsed * 2;
        };

        if ((map["q"] || map["Q"])) {
            zRot += elapsed * 2/*turnSpeed*/;
        }

        if ((map["e"] || map["E"])) {
            zRot -= elapsed * 2/*turnSpeed*/;
        }

        if (map["ArrowUp"]) {
            //tiefighter.movePOV(0, 0, 10);
        }

        if (map["ArrowDown"]) {
            //tiefighter.movePOV(0, 0, -10);
        }

        if ((map[" "])) {
            fireFrom(tiefighter);
        }

        if ((map["Escape"])) {
            pauseGame();
        }

        // Keep skybox from rotating
        //skybox.rotation.x = 0;
        //skybox.rotation.y = 0;
        //skybox.rotation.z = 0;

        // Example code
        //var elapsed = engine.getDeltaTime() / 1000;

        //// handle keys here
        //if (keysDown[87]) {
        //    // W, rotate in the negative direction about the x axis
        //    xRot += elapsed * turnSpeed;
        //}

        //if (keysDown[83]) {
        //    // S, rotate in the positive direction about the x axis
        //    xRot -= elapsed * turnSpeed;
        //}

        //if (keysDown[65]) {
        //    // A, rotate left
        //    yRot -= elapsed * turnSpeed;
        //    if (autocoord) {
        //        zRot += elapsed * turnSpeed;
        //    }
        //}

        //if (keysDown[68]) {
        //    // D, rotate right
        //    yRot += elapsed * turnSpeed;
        //    if (autocoord) {
        //        zRot -= elapsed * turnSpeed;
        //    }
        //}

        //if (keysDown[81]) {
        //    // Q, rotate in the positive direction about the z axis
        //    zRot += elapsed * turnSpeed;
        //}

        //if (keysDown[69]) {
        //    // E, rotate in the negative direction about the z axis
        //    zRot -= elapsed * turnSpeed;
        //}

        //var craft = scene.getMeshByName("craftparent");
        //var cam = scene.getCameraByID("Camera");

        //tiefighter.translate(BABYLON.Axis.Z, elapsed + tiefighter.accelerations.forward, BABYLON.Space.LOCAL);
        tiefighter.translate(BABYLON.Axis.Z, -1 * (elapsed + 1), BABYLON.Space.LOCAL);
        tiefighter.rotation = new BABYLON.Vector3(xRot, yRot, zRot);
        //tiefighter.physicsImpostor.applyImpulse(new BABYLON.Vector3(xRot * elapsed, yRot * elapsed, zRot * elapsed), tiefighter);
        //craft.translate(BABYLON.Axis.Z, elapsed + airSpeed, BABYLON.Space.LOCAL);
        //craft.rotation = new BABYLON.Vector3(xRot, yRot, zRot);
    });
}

function loadShip(shipFileName) {
    XMLHttpRequestPromise("GET", shipFileName)
        .then(function (response) {
            var parsedJSON = JSON.parse(response);
            console.log(parsedJSON);
        }).catch(function (error) {
            console.warn(`Failed to load ship ${shipFileName}`);
            console.error(error);
        })
}

/**
 * Checks if the modal is already loaded, if not imports the modal and stores
 * it.
 * @param {any} modalFileName The name of the modal to load
 */
function loadMeshModal(modalFileName) {
    return new Promise(function (resolve, reject) {
        var foundMesh = false;
        var meshModalName = modalFileName.replace(".babylon", "Modal");
        var parentMesh = {};

        for (var loadedMesh of loadedMeshes) {
            if (loadedMesh.name == meshModalName) {
                foundMesh = true;
                parentMesh = loadedMesh;
                break;
            }
        }

        if (foundMesh) {
            resolve(parentMesh);
        } else {
            try {
                BABYLON.SceneLoader.ImportMesh("", "/models/", modalFileName, null, function (newMesh) {
                    parentMesh = newMesh[0];
                    for (var i = 1; i < newMesh.length; i++) {
                        newMesh[i].parent = parentMesh;
                    }

                    loadedMeshes.push(parentMesh);
                    resolve(newMesh);
                });
            } catch (error) {
                reject(error);
            }
        }
    });
}

function unloadMeshModal(modalFileName) {
    var foundMesh = false;

    for (var loadedMesh of loadedMeshes) {
        if (loadedMesh.name == meshModalName) {
            foundMesh = true;
            break;
        }
    }

    return foundMesh;
}

var lasers = [];
var canFire = true;
var burnOutVal = 0;
var maxBurnOut = 10;
function fireFrom(source) {
    if (canFire && (burnOutVal < maxBurnOut)) {
        function unlockFire() {
            setTimeout(function () {
                canFire = true;
            }, 200);
        }

        burnOutVal++;
        canFire = false;
        unlockFire();

        console.log("firing laser");
        var forwardRay = camera.getForwardRay();
        let laser = BABYLON.MeshBuilder.CreateCylinder("laser", { diameter: 1, tessellation: 4, height: 10 }, scene);
        laser.physicsImpostor = new BABYLON.PhysicsImpostor(laser, BABYLON.PhysicsImpostor.CylinderImpostor, { mass: 1, restitution: 0.9 }, scene);
        laser.rotation.x = Math.PI / 4;
        var pos = source.getAbsolutePosition();
        laser.setAbsolutePosition(pos);
        //laser.rotation = forwardRay.direction;
        //cylinder.addRotation(0, Math.PI / 2);
        //cylinder.rotation.x = camera.rotation.x;
        //cylinder.rotation.y = camera.rotation.y + Math.PI/2;
        lasers.push(laser);

        //var invView = new BABYLON.Matrix();
        //camera.getViewMatrix().invertToRef(invView);
        //var direction = BABYLON.Vector3.TransformNormal(new BABYLON.Vector3(0, 0, 1), invView);

        //direction.normalize();

        //scene.registerBeforeRender(function () {
        //    bullet.position.addInPlace(direction);
        //});

        var laserVelocity = new BABYLON.Vector3(0, 0, -100);
        laserVelocity
        laser.physicsImpostor.setLinearVelocity(new BABYLON.Vector3(0, 0, -100));
        var laserMaterial = new BABYLON.StandardMaterial("laser", scene);
        laserMaterial.emissiveColor = new BABYLON.Color3(0,1,0);
        //laserMaterial.ambientColor = new BABYLON.Color3(66, 244, 78);
        laserMaterial.ambientColor = new BABYLON.Color3(0, 1, 0);
        laserMaterial.diffuseColor = new BABYLON.Color3(0, 1, 0);
        laserMaterial.specularColor = new BABYLON.Color3(0, 1, 0);
        laserMaterial.specularPower = 32;
        laser.material = laserMaterial;
        var disposeFunc = function (laser) {
            laser.dispose();
            console.log("disposing func");
            burnOutVal--;
        };
        setTimeout(disposeFunc, 5000, laser);
    }
}

var isPaused = false;
function pauseGame() {
    if (!isPaused) {
        isPaused = true;
        engine.stopRenderLoop();
        Game.menus.inGameMenu.show();
    }
}

function resumeGame() {
    if (isPaused) {
        isPaused = false;
        Game.menus.inGameMenu.hide();
        engine.runRenderLoop(function () {
            scene.render();
        });
    }    
}