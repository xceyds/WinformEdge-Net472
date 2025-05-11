$(document).ready(function () {
    var number = "";
    var numberTwo = "";
    var selectedOperator = "";
    var result = $("#display");


    // clear display
    $('.clear').click(function () {
        number = "";
        numberTwo = "";
        selectedOperator = "";
        $('#display').text("0");
    })

    //sloppy +/- change
    function signChange() {
        number *= -1;
        $('#display').text(number);
        console.log("signchange: " + number);
    }
    var pnsignClick = document.getElementById("pnsign");
    pnsignClick.addEventListener("click", signChange);


    //enter numbers and decimal
    $('.number').click(function () {
        number += $(this).text();
        $('#display').text(number);
        console.log(number);
    })

    $(".operator").not("#eval").click(function () {
        selectedOperator = $(this).text();
        numberTwo = number;
        number = "";
        console.log("op:" + selectedOperator);
        console.log("2:" + numberTwo);
        console.log("1:" + number);
        //result.text("");
    });

    //do the operation
    $('#eval').click(function () {
        if (!numberTwo) numberTwo = 0;
        if (!number) number = 0;

        if (selectedOperator === "+") {
            number = (parseFloat(numberTwo) + parseFloat(number));
        } else if (selectedOperator === "-") {
            number = (parseFloat(numberTwo) - parseFloat(number));
        } else if (selectedOperator === "×") {
            number = (parseFloat(numberTwo) * parseFloat(number)).toPrecision(5);
        } else if (selectedOperator === "÷") {
            number = (parseFloat(numberTwo) / parseFloat(number)).toPrecision(5);
        } else if (selectedOperator === "xy") {
            number = (Math.pow(numberTwo, number)).toPrecision(5);
        } else if (selectedOperator === "√") {
            number = Math.sqrt(numberTwo);
        }

        if ((number % 1) === 0) {
            number = parseInt(number);
        }

        result.text(number);
        number = "";
        numberTwo = "";

    })

});