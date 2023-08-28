$(document).ready(function () {
    // Функция для загрузки и отображения продуктов
    function loadProducts() {
        $.ajax({
            url: "/Product/GetProducts", // Замените на ваш реальный URL
            type: "GET",
            dataType: "json",
            success: function (response) {
                var productContainer = $("#productContainer");
                productContainer.empty(); // Очищаем контейнер перед вставкой новых карточек

                // Создаем и добавляем карточки продуктов
                $.each(response, function (index, product) {
                    var card = $("<div>").addClass("col-md-4");
                    var cardInner = $("<div>").addClass("card");
                    var cardImage = $("<img>").addClass("card-img-top").attr("src", "https://www.paperseal.lt/media/wysiwyg/productimages/K312221_2.jpg").attr("alt", product.name);
                    var cardBody = $("<div>").addClass("card-body");

                    $("<h5>").addClass("card-title").text(product.name).appendTo(cardBody);
                    $("<p>").addClass("card-text").text(product.description).appendTo(cardBody);
                    $("<p>").addClass("card-text").text("Стоимость: " + product.price + " ₽").appendTo(cardBody);

                    var detailsButton = $("<a>").addClass("btn btn-primary").attr("href", "/Product/GetProduct/" + product.id).text("Подробнее");
                    detailsButton.appendTo(cardBody);

                    cardImage.appendTo(cardInner);
                    cardBody.appendTo(cardInner);
                    cardInner.appendTo(card);

                    card.appendTo(productContainer);
                });
            },
            error: function () {
                console.log("Ошибка при загрузке продуктов");
            }
        });
    }

    // Загрузка продуктов при загрузке страницы
    loadProducts();
});

