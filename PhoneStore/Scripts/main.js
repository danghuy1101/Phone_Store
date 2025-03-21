document.addEventListener("DOMContentLoaded", function () {
    const minusBtn = document.getElementById("counter-minus");
    const plusBtn = document.getElementById("counter-plus");
    const quantityInput = document.getElementById("quantity_product");

    plusBtn.addEventListener("click", function () {
        let currentValue = parseInt(quantityInput.value);
        if (currentValue < 3) {  // Giới hạn tối đa là 3
            quantityInput.value = currentValue + 1;
        }
    });

    minusBtn.addEventListener("click", function () {
        let currentValue = parseInt(quantityInput.value);
        if (currentValue > 1) {  // Giới hạn tối thiểu là 1
            quantityInput.value = currentValue - 1;
        }
    });

    // Đảm bảo người dùng không nhập số vượt quá 3 hoặc nhỏ hơn 1
    quantityInput.addEventListener("input", function () {
        let currentValue = parseInt(quantityInput.value);
        if (currentValue > 3) {
            quantityInput.value = 3;
        } else if (currentValue < 1 || isNaN(currentValue)) {
            quantityInput.value = 1;
        }
    });
});
