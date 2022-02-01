// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const CART_COOKIE_NAME = "cart";
const DEFAULT_CART_EXDAYS = 7;
const DEFAULT_CART = "{}";
const DEFAULT_PRODUCT_QUANTITY = 0;

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for(let i = 0; i <ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
        c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
        return c.substring(name.length, c.length);
        }
    }
    return "";
    }

function setCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays*24*60*60*1000));
    let expires = "expires="+ d.toUTCString();
    document.cookie = cname + "=" + encodeURIComponent(cvalue) + ";" + expires + ";path=/";
}

if(getCookie(CART_COOKIE_NAME) == ""){
    setCookie(CART_COOKIE_NAME, DEFAULT_CART, DEFAULT_CART_EXDAYS);
}

function removeFromCart(product_id){
    let cart_str = getCookie(CART_COOKIE_NAME);
    let cart_json = JSON.parse(cart_str);
    delete cart_json[product_id];
    let edited_cart_str = JSON.stringify(cart_json);
    setCookie(CART_COOKIE_NAME, edited_cart_str, DEFAULT_CART_EXDAYS);
    updateCartLabel();
}

function setProductQuantity(product_id, new_value = DEFAULT_PRODUCT_QUANTITY){
    let cart_str = getCookie(CART_COOKIE_NAME);
    let cart_json = JSON.parse(cart_str);
    cart_json[product_id] = new_value;
    let edited_cart_str = JSON.stringify(cart_json);
    setCookie(CART_COOKIE_NAME, edited_cart_str, DEFAULT_CART_EXDAYS)
}

function getProductQuantity(product_id){
    let cart_str = getCookie(CART_COOKIE_NAME);
    let cart_json = JSON.parse(cart_str);
    return cart_json[product_id] ?? 0;
}

function updateCost(product_id_str){
    let quantity = document.getElementById(product_id_str + "_quantity").value;
    if(quantity < 1){
        let cart_rows = document.getElementById("cart_rows");
        let product_row = document.getElementById(product_id_str + "_row");
        cart_rows.removeChild(product_row);
        removeFromCart(product_id_str);
    } else {
        let money = parseFloat(document.getElementById(product_id_str + "_money").firstChild.textContent);
        setProductQuantity(product_id_str, parseInt(quantity));
        let cost = money * quantity;
        document.getElementById(product_id_str + "_cost").textContent = parseFloat(cost).toFixed(2).toString();
    }
    updateSummaryCost();
}

function updateCartLabel(){
    let cart_rows = document.getElementById("cart_rows");
    document.getElementById("empty_cart").hidden = cart_rows.childElementCount != 0;
    document.getElementById("cart").hidden = cart_rows.childElementCount == 0;
}

function updateSummaryCost(){
    let cart_rows = document.getElementById("cart_rows");
    let summary = 0;
    cart_rows.childNodes.forEach(child => {
        let row_id = child.id;
        if(row_id != undefined)
        {
            let id = row_id.substring(0, row_id.length - 4);    
            summary += parseFloat(document.getElementById(id + "_cost").textContent);
        }
    })
    document.getElementById("summary_cost").textContent = parseFloat(summary).toFixed(2).toString();
}

function updateAllCosts(){
    let cart_rows = document.getElementById("cart_rows");
    cart_rows.childNodes.forEach(child => {
        let row_id = child.id;
        if(row_id != undefined)
        {
            let id = row_id.substring(0, row_id.length - 4);
            updateCost(id);
        }
    })
    updateSummaryCost();
}

function clearCart(){
    setCookie(CART_COOKIE_NAME, DEFAULT_CART, DEFAULT_CART_EXDAYS);
}

let loading_articles = false;
let end_of_data = false;

function loadArticles(){
    if(!loading_articles && (!end_of_data)){
        loading_articles = true;
        let ids = document.getElementsByClassName("article_id");
        let last_id = ids[ids.length - 1].textContent;
        $.ajax({
                type: "GET",
                url: "/api/article/?start_id=" + last_id + "&number=5",
                success: function (articles) {
                    if(articles.length == 0){
                        end_of_data = true;
                    }
                    setTimeout(function(){
                        articles.forEach(article => {
                            let tr = $('<tr></tr>');
                            $("<td class='article_id' hidden>" + article.articleId + "</td>").appendTo(tr);
                            $("<td>" + article.name + "</td>").appendTo(tr);
                            $("<td>" + article.money + "</td>").appendTo(tr);
                            $("<td>" + article.category.name + "</td>").appendTo(tr);
                            console.log(article.imageName);
                            if(article.imageName == null){
                                $("<td><img height='100px' src='image/placeholder.jpg'/></td>").appendTo(tr);
                            } else {
                                $("<td><img height='100px' src='upload/" + article.imageName + "'/></td>").appendTo(tr);
                            }
                            $("<td><button class='btn btn-light' onclick='setProductQuantity(parseInt(" + article.articleId + "), getProductQuantity(parseInt(" + article.articleId + ")) + 1)'>Add to cart</button></td>").appendTo(tr);
                            tr.appendTo($('#articles_table_body'));
                        });
                        loading_articles = false;
                    }, 1000);
                }
            });
    }
}