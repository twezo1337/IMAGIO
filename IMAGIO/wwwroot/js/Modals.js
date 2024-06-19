const auth_btn = document.getElementsByClassName("auth_btn")[0];
const reg_btn = document.getElementsByClassName("reg_btn")[0];

const bodyElementHTML = document.getElementsByTagName("body")[0];
const modalAuthBackground = document.getElementsByClassName("modalAuthBackground")[0];
const modalAuthClose = document.getElementsByClassName("modalAuthClose")[0];
const modalAuthActive = document.getElementsByClassName("modalAuthActive")[0];

const modalRegBackground = document.getElementsByClassName("modalRegBackground")[0];
const modalRegClose = document.getElementsByClassName("modalRegClose")[0];
const modalRegActive = document.getElementsByClassName("modalRegActive")[0];


auth_btn.addEventListener("click", function () {
    modalAuthBackground.style.display = "block";
});

modalAuthClose.addEventListener("click", function () {
    modalAuthBackground.style.display = "none";
});

modalAuthBackground.addEventListener("click", function (event) {
    if (event.target === modalAuthBackground) {
        modalAuthBackground.style.display = "none";
    }
});

reg_btn.addEventListener("click", function () {
    modalRegBackground.style.display = "block";
});

modalRegClose.addEventListener("click", function () {
    modalRegBackground.style.display = "none";
});

modalRegBackground.addEventListener("click", function (event) {
    if (event.target === modalRegBackground) {
        modalRegBackground.style.display = "none";
    }
});