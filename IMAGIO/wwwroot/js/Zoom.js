const plusZoom = document.getElementById("plus");
const minusZoom = document.getElementById("minus");
let frameImage = document.getElementById("frameImage");
let i = 1;

console.log('1');
plusZoom.addEventListener("click", function () {
    i = i + 0.1;
    frameImage.style.transform = `scale(${i})`;
});

minusZoom.addEventListener("click", function () {
    i = i - 0.1;
    frameImage.style.transform = `scale(${i})`;
});

