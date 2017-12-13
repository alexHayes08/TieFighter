//$(function () {
    var shipMeshes = [];

    function Coords(x, y, z) {
        this.x = x || 0;
        this.y = y || 0;
        this.z = z || 0;
    }

    function Submesh(name, tOffset, rOffset, sOffset) {
        this.name = name;
        this.translationOffset = tOffset || new Coords();
        this.rotationOffset = rOffset || new Coords();
        this.scaleOffset = sOffset || new Coords();
    }

    // For Babylon preview
    var canvas = $("#shipPreview")[0];
    var engine = new BABYLON.Engine(canvas, true);
    var scene = null;
    var camera = null;
    var light = null;

    function resetScene() {
        scene.meshes[scene.meshes.length - 1].dispose();
    }

    function loadModal(url) {
        if (scene == null) {
            scene = new BABYLON.Scene(engine);
            scene.clearColor = BABYLON.Color3.Black();
        }
        if (light == null) {
            var light = new BABYLON.PointLight("Omni", new BABYLON.Vector3(0, 100, 100), scene);
        }
        if (camera == null) {
            camera = new BABYLON.ArcRotateCamera("Camera", 0, 0.8, 100, BABYLON.Vector3.Zero(), scene);
            camera.attachControl(canvas, false);
        }

        if (light == null) {
            light = new BABYLON.HemisphericLight("light1", new BABYLON.Vector3(0, 1, 0), scene);
        }

        var fileName = `${$("#Id").val()}%20.babylon`;

        if (path == null) {
            BABYLON.SceneLoader.ImportMesh("", "/resources/Models/", fileName, scene, function (newMesh) {

                tiefighter = BABYLON.MeshBuilder.CreateBox("tiefighterParent", {}, scene);
                tiefighter.visibility = 0;
                for (var i = 0; i < newMesh.length; i++) {
                    var submesh = new Submesh(newMesh[i].id);
                    shipMeshes.push(submesh);

                    console.log(newMesh);
                    newMesh[i].parent = tiefighter;
                }

                tiefighter.rotation.y = Math.PI;

                //camera.parent = tiefighter;

                // Scene starts
                engine.runRenderLoop(function () {
                    var canvasSize = $("#shipPreviewContainer > div").width();
                    engine.setSize(canvasSize, canvasSize);
                    scene.render();
                    isLoading($("#shipPreviewContainer > div")[0], false);
                });
            }, null, function (error) {
                new jBox('Notice', {
                    content: "Failed to load ship modal",
                    color: "red"
                }).open();
            });
        } else {
            BABYLON.SceneLoader.ImportMesh("", "/resources/Models/", fileName, scene, function (newMesh) {

                tiefighter = BABYLON.MeshBuilder.CreateBox("tiefighterParent", {}, scene);
                tiefighter.visibility = 0;
                for (var i = 0; i < newMesh.length; i++) {
                    var submesh = new Submesh(newMesh[i].id);
                    shipMeshes.push(submesh);

                    console.log(newMesh);
                    newMesh[i].parent = tiefighter;
                }

                tiefighter.rotation.y = Math.PI;

                //camera.parent = tiefighter;

                // Scene starts
                engine.runRenderLoop(function () {
                    var canvasSize = $("#shipPreviewContainer > div").width();
                    engine.setSize(canvasSize, canvasSize);
                    scene.render();
                    isLoading($("#shipPreviewContainer > div")[0], false);
                });
            }, null, function (error) {
                new jBox('Notice', {
                    content: "Failed to load ship modal",
                    color: "red"
                }).open();
            });
        }
    }

    $("#updateEntity").on("click", function () {
        shipMeshes = [];

        var $inputs = $("body main input, body main textarea, body main select, body main button");

        function show() {
            setInputsTo($inputs, true);
            isLoading($("body main form")[0], false);
            $("#updateEntity").removeAttr("disabled");
        }

        isLoading($("body main form")[0], true);
        var formData = new FormData($("form")[0]);

        $("#updateEntity").attr("disabled", "");
        setInputsTo($inputs, false);

        XMLHttpRequestPromise("POST", window.location.href, formData)
            .then(function (response) {
                var res = JSON.parse(response);
                if (!res.succeeded) {
                    throw new Error(res.Error);
                }
                show();
                new jBox('Notice', {
                    content: "Successfully updated.",
                    color: "green"
                }).open();
            }).catch(function (error) {
                show();
                showErrorMsg(error);
                new jBox('Notice', {
                    content: "Failed to update!",
                    color: "red"
                }).open();
            });
    });

    $("#shipPreviewContainer").on("shown.bs.collapse", function () {
        $(this).off("shown.bs.collapse");

        loadModal();
    });

    $("#newBabylonModel").on("change", function () {
        if (this.value) {
            var file = this.files[0];
            var reader = new FileReader();
            reader.onload = function () {
                
            }
            reader.readAsDataURL(file);
        }
    });
//});

//BABYLON.Tools.LoadFile(sceneBlob, (data: string) => {
//    binaries.forEach((binary) => {
//        var re = new RegExp(binary.originalName, "g");
//        data = data.replace(re, binary.newUrl);
//    });
//    BABYLON.SceneLoader.ImportMesh("", "", "data:" + data, this._scene, (meshes: BABYLON.AbstractMesh[]) => {
//        meshes.forEach((mesh) => {
//            if (!mesh.material)
//                mesh.material = new BABYLON.StandardMaterial(mesh.name + "Mat", this._scene);
//            mesh.actionManager = new BABYLON.ActionManager(this._scene);
//            mesh.actionManager.registerAction(new BABYLON.SetValueAction(BABYLON.ActionManager.OnPointerOutTrigger, mesh, "renderOutline", false));
//            mesh.actionManager.registerAction(new BABYLON.SetValueAction(BABYLON.ActionManager.OnPointerOverTrigger, mesh, "renderOutline", true));
//            mesh.actionManager.registerAction(new BABYLON.ExecuteCodeAction(BABYLON.ActionManager.OnRightPickTrigger, (evt: BABYLON.ActionEvent) => {
//                this.selectObject(mesh, true);
//            }));
//        });
//        this.$rootScope.$broadcast("sceneReset");
//    });
//});