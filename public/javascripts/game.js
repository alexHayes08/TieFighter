window.addEventListener("DOMContentLoaded", function () {
    var canvas = document.getElementById("scene");
    var engine = new BABYLON.Engine(canvas, true);

    var createScene = function () {
        // var scene = new BABYLON.Scene(engine);
        // var camera = new BABYLON.FreeCamera("MainCamera", new BABYLON.Vector3(0,5,-10));

        var scene = new BABYLON.Scene(engine);
        var light = new BABYLON.PointLight("Omni", new BABYLON.Vector3(0, 100, 100), scene);
        var camera = new BABYLON.ArcRotateCamera("Camera", 0, 0.8, 100, BABYLON.Vector3.Zero(), scene);

        camera.setTarget(BABYLON.Vector3.Zero());
        camera.attachControl(canvas, false);
        var light = new BABYLON.HemisphericLight("light1", new BABYLON.Vector3(0,1,0), scene);
        // var sphere = BABYLON.Mesh.CreateSphere("sphere1", 16, 2, scene);
        // sphere.position.y = 1;
        // var cube = BABYLON.Mesh.CreateBox("box", 2.0, scene, false, BABYLON.Mesh.DEFAULTSIDE);
        // cube.position.y = 1;
        // cube.position.x = 3;

        var sphere1 = BABYLON.Mesh.CreateSphere("Sphere1", 10.0, 6.0, scene);
        var sphere2 = BABYLON.Mesh.CreateSphere("Sphere2", 2.0, 7.0, scene);
        var sphere3 = BABYLON.Mesh.CreateSphere("Sphere3", 10.0, 8.0, scene);
        var sphere4 = BABYLON.Mesh.CreateSphere("Sphere4", 10.0, 8.0, scene);
        var sphere5 = BABYLON.Mesh.CreateSphere("Sphere5", 10.0, 8.0, scene);
        var sphere6 = BABYLON.Mesh.CreateSphere("Sphere6", 10.0, 8.0, scene);

        sphere1.position.x = -40;
        sphere2.position.x = -30;
        sphere3.position.x = -5;
        sphere4.position.x = 5;
        sphere5.position.x = 30;
        sphere6.position.x = 40;

        var material1 = new BABYLON.StandardMaterial("texture1", scene);
        material1.alpha = 0.5;
        material1.diffuseColor = new BABYLON.Color3(1, 0.2, 0.7);
        material1.emissiveColor = new BABYLON.Color3(1, .2, .7);
        material1.ambientColor = new BABYLON.Color3(1,0.2,0.7);
        material1.specularColor = new BABYLON.Color3(1, 0.2, 0.7);
        material1.specularPower = 32;

        sphere1.material = material1;
        // sphere2.material = new BABYLON

        var ground = BABYLON.Mesh.CreateGround("ground1", 100, 100, 0, scene);

        var animationBox = new BABYLON.Animation("myAnimation", "scaling.x", 30, BABYLON.Animation.ANIMATIONTYPE_FLOAT, BABYLON.Animation.ANIMATIONLOOPMODE_CYCLE);
        var keys = [];
        keys.push({
            frame: 0,
            value: 1
        });

        keys.push({
            frame: 20,
            value: 0.2
        });

        keys.push({
            frame: 100,
            value: 1
        });

        animationBox.setKeys(keys);
        sphere1.animations = [];
        sphere1.animations.push(animationBox);

        scene.beginAnimation(sphere1, 0, 100, true);

        return scene;
    }

    var scene = createScene();

    engine.runRenderLoop(function () {
        scene.render();
    });
});