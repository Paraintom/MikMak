$("#BTdate").click(DateClick);

function DateClick() {
    var val;

    $.ajax({
        url: "/game/samplegame/",
        type:"GET",
        datatype: "json",
        data: "",
        success: GetDateSuccess
    });
}


function GetDateSuccess(data){
    $("#dateSpan").text(data);
}
/****************************************************************************************/
$("#BTcomplexData").click(ComplexClick);

function ComplexClick() {
    var val;

    $.ajax({
        url: "/game/samplegame/",
        type: "GET",
        datatype: "json",
        data: {id:2},
        success: GetComplexSuccess
    });
}


function GetComplexSuccess(data) {
    $("#complexData").text(data);
}