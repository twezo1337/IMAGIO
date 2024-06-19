window.onload = function () {
    var canvas = document.getElementById("canvas");
    var ctx = canvas.getContext("2d");
    var isDrawing = false;
    var offsetX = 0;
    var offsetY = 0;

    var frameImage = document.getElementById("frameImage");
    var canvasContainer = document.querySelector(".edit_frame");

    canvas.width = frameImage.width;
    canvas.height = frameImage.height;

    ctx.drawImage(frameImage, 0, 0, canvas.width, canvas.height);

    var rect = canvas.getBoundingClientRect(); // ѕолучаем позицию и размеры холста относительно окна браузера
    offsetX = rect.left;
    offsetY = rect.top;

    canvas.onmousedown = function (e) {
        isDrawing = true;
        var mouseX = e.clientX - offsetX + canvasContainer.scrollLeft;
        var mouseY = e.clientY - offsetY + canvasContainer.scrollTop;
        ctx.beginPath();
        ctx.moveTo(mouseX, mouseY);
        ctx.strokeStyle = "red";
        ctx.lineWidth = 5;
    };

    canvas.onmousemove = function (e) {
        if (!isDrawing) return;
        var mouseX = e.clientX - offsetX + canvasContainer.scrollLeft;
        var mouseY = e.clientY - offsetY + canvasContainer.scrollTop;
        ctx.lineTo(mouseX, mouseY);
        ctx.stroke();
    };

    canvas.onmouseup = function () {
        isDrawing = false;

        // ѕолучаем данные изображени€ в формате base64
        var imageData = canvas.toDataURL("image/jpg");

        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/PhotoEditor/UploadDrawingImage", true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    console.log("OK!");
                    var response = JSON.parse(xhr.responseText);
                    window.location.href = response.redirectTo;
                } else {
                    console.error("Request failed with status:", xhr.status);
                }
            }
        };


        xhr.send(JSON.stringify({ image: imageData }));

    };
};