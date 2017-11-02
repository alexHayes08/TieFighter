function loadingScreen (show) {
    show = Boolean(show);
    document.getElementById("mainLoading").style.display = show ? "block" : "none";
}

class Mission {
    constructor () {

    }
    get name () {

    }
    get missionBriefing () {
        
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

var TieFighter = {
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

var canvas = {};
var engine = {};
var scene = {};
var camera = {};
var cameras = [];
var tiefighter = {};
window.addEventListener("DOMContentLoaded", function () {
    // First init firebase & check if user is logged in
    var config = {
        apiKey: "AIzaSyCgeNBTA_rTNn0NG2nCAYj401Kf5rXRgx0",
        authDomain: "tiefighter-imperialremnant.firebaseapp.com",
        databaseURL: "https://tiefighter-imperialremnant.firebaseio.com",
        projectId: "tiefighter-imperialremnant",
        storageBucket: "tiefighter-imperialremnant.appspot.com",
        messagingSenderId: "1086585271464"
      };
      firebase.initializeApp(config);
      firebase.auth().setPersistence(firebase.auth.Auth.Persistence.LOCAL).then(function () {
        firebase.auth().onAuthStateChanged(function(user) {
            if (user) {
                // Continue loading game
                console.log("User is logged in.")
                // loadGame();
                
                // Show menu
                TieFighter.menus.mainMenu.show();
                loadingScreen(false);
            } else {
                // Display message about needing to login
                console.error("User isn't logged in")
                loadingScreen(true);
                $("#messageModal").show();
            }
        });
      });
});

function loadGame () {

    canvas = document.getElementById("scene");
    engine = new BABYLON.Engine(canvas, true);
    
    // var loader = new BABYLON.AssetsManager(canvas);
    // var tiefighter = loader.addMeshTask("tiefighter", "", "/public", "tiefighter.stl")

    var createScene = function () {
        // var scene = new BABYLON.Scene(engine);
        // var camera = new BABYLON.FreeCamera("MainCamera", new BABYLON.Vector3(0,5,-10));

        scene = new BABYLON.Scene(engine);
        var light = new BABYLON.PointLight("Omni", new BABYLON.Vector3(0, 100, 100), scene);
        camera = new BABYLON.ArcRotateCamera("Camera", 0, 0.8, 100, BABYLON.Vector3.Zero(), scene);

        camera.setTarget(BABYLON.Vector3.Zero());
        camera.attachControl(canvas, false);
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

        BABYLON.SceneLoader.ImportMesh("", "", "tiefighter.obj", scene, function (newMesh) {
            // camera.setTarget(newMesh);
            tiefighter = newMesh[0];
            for (var i = 1; i < newMesh.length; i++)
                newMesh[i].parent = tiefighter;
            console.log(newMesh);
        });

        return scene;
    }

    var scene = createScene();

    // Scene starts
    engine.runRenderLoop(function () {
        scene.render();
    });
}