$(function () {
    $("#updateEntity").on("click", function () {

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

        // Create Babylon preview
        var canvas = $("#shipPreview")[0];
        var engine = new BABYLON.Engine(canvas, true);
        var scene = null;
        var camera = null;

        var createScene = function () {

            scene = new BABYLON.Scene(engine);
            var gravityVector = new BABYLON.Vector3(0, 0, 0);
            scene.enablePhysics(gravityVector);
            var light = new BABYLON.PointLight("Omni", new BABYLON.Vector3(0, 100, 100), scene);
            camera = new BABYLON.ArcRotateCamera("Camera", 0, 0.8, 100, BABYLON.Vector3.Zero(), scene);
            camera.attachControl(canvas, false);
            scene.clearColor = BABYLON.Color3.Black();
            var light = new BABYLON.HemisphericLight("light1", new BABYLON.Vector3(0, 1, 0), scene);

            var fileName = $("#FileLocation").val();
            BABYLON.SceneLoader.ImportMesh("", "/models/", fileName, scene, function (newMesh) {

                tiefighter = BABYLON.MeshBuilder.CreateBox("tiefighterParent", {}, scene);
                tiefighter.visibility = 0;
                for (var i = 0; i < newMesh.length; i++) {
                    newMesh[i].parent = tiefighter;
                }

                tiefighter.rotation.y = Math.PI;

                camera.parent = tiefighter;

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

            return scene;
        }

        scene = createScene();
    })
});