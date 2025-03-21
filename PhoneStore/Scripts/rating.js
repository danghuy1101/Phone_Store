document.addEventListener("DOMContentLoaded", function () {
    const stars = document.querySelectorAll(".star");
    const ratingInput = document.getElementById("selected-rating");

    stars.forEach((star, index) => {
        star.addEventListener("mouseover", function () {
            resetStars();
            highlightStars(index + 1);
        });

        star.addEventListener("click", function () {
            ratingInput.value = index + 1;
            persistStars(index + 1);
        });
    });

    function resetStars() {
        stars.forEach(star => {
            star.classList.remove("active");
        });
    }

    function highlightStars(rating) {
        for (let i = 0; i < rating; i++) {
            stars[i].classList.add("active");
        }
    }

    function persistStars(rating) {
        resetStars();
        highlightStars(rating);
    }
});
