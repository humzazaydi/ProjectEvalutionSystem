$("input[type='text']:visible").each(function () {
    var textlengthcheck = $(this).val();

    if (textlengthcheck.length > 0) {
        $(this).parent().addClass("input--filled")
    }
});
$(document).ready(function () {

    $('#divAreaPasswordChange').hide(500);

    FormValidationEvents()

    //toaster
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": true,
        "showDuration": "300",
        "hideDuration": "800",
        "timeOut": "5000",
        "extendedTimeOut": "10000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "slideDown",
        "hideMethod": "slideUp"
    };



});
function FormValidationEvents() {
    $(document).on('change', 'input[type="text"] , input[type="password"],input[type="email"]', function () {
        $(this).val($(this).val().trim())
        if ($(this).attr("data-readonly") !== "true" && $(this).attr("data-required") == "true") {
            if ($(this).val() == '') {


                $(this).addClass('input-validation-error');
                $(this).next("span#RequiredLable").remove();
                $("<span id='RequiredLable' style='color:red;'><small>required</small></span>").insertAfter($(this))

                return false;
            }
            else {
                $(this).next("span#RequiredLable").remove();
                $(this).removeClass("input-validation-error")
            }
        }
        if ($(this).attr("data-readonly") !== "true" && $(this).attr("data-number") == "true") {
            var pattern = /^[0-9]+(\.[0-9]{1,2})?$/;  // /^\d+$/;
            if ($(this).val() !== "" && !$.isNumeric($(this).val())) {
                $(this).addClass('input-validation-error');
                $(this).next("span").remove();
                $("<span style='color:red;'><small>Number required</small></span>").insertAfter($(this))
                var newVal = $(this).val().replace(/[^0-9.,]/g, '')
                $(this).val(newVal);
                return false;
            }
        }
        if ($(this).attr("data-readonly") !== "true" && $(this).attr("minlength")) {
            if ($(this).val() !== "" && $(this).val().length < $(this).attr("minlength")) {

                $(this).addClass('input-validation-error');
                $(this).next("span#RequiredLable").remove();
                $("<span id='RequiredLable' style='color:red;'><small>Minimum " + (typeof $(this).attr("masklength") !== "undefined" && $(this).attr("masklength").length > 0 ? $(this).attr("minlength") - $(this).attr("masklength") : $(this).attr("minlength")) + " length required</small></span>").insertAfter($(this))
                return false;
            }
            else {
                $(this).next("span#RequiredLable").remove();
                $(this).removeClass("input-validation-error")

            }

        }
        if ($(this).attr("data-readonly") !== "true" && $(this).attr("maxlength")) {
            if ($(this).val() !== "" && $(this).val().length < $(this).attr("maxlength")) {

                return false;
            }
        }
    });
    $("input[type='text'] , input[type='password'], input[type='email']").on({
        focus: function () {
            $(this).next("span#RequiredLable").remove();
            $(this).removeClass("input-validation-error")
        },
        blur: function () {
            $(this).val($(this).val().trim())
            if ($(this).attr("data-readonly") !== "true" && $(this).attr("data-required") == "true") {
                if ($(this).val() == '') {

                    $(this).addClass('input-validation-error');
                    $(this).next("span#RequiredLable").remove();
                    $("<span id='RequiredLable' style='color:red;'><small>required</small></span>").insertAfter($(this))

                    return false;
                }
                else {
                    $(this).next("span#RequiredLable").remove();
                    $(this).removeClass("input-validation-error")
                }
            }
            if ($(this).attr("data-readonly") !== "true" && $(this).attr("data-number") == "true") {
                var pattern = /^[0-9]+(\.[0-9]{1,2})?$/;  // /^\d+$/;
                if ($(this).val() !== "" && !$.isNumeric($(this).val())) {
                    $(this).addClass('input-validation-error');
                    $(this).next("span").remove();
                    $("<span style='color:red;'><small>Number required</small></span>").insertAfter($(this))
                    var newVal = $(this).val().replace(/[^0-9.,]/g, '')
                    $(this).val(newVal);
                    return false;
                }
            }
            if ($(this).attr("data-readonly") !== "true" && $(this).attr("data-email") == "true") {

                $(this).val($(this).val().trim())
                var val = $(this).val();
                var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                var tes = regex.test(val);
                if (val != "" && !tes) {
                    $(this).addClass('input-validation-error');
                    $(this).next("span#RequiredLable").remove();
                    $("<span id='RequiredLable' style='color:red;'><small>Invalid email format</small></span>").insertAfter($(this))
                    return false;
                }
                else {
                    $(this).removeClass('input-validation-error');
                    $(this).next("span").remove();
                }
            }
            if ($(this).attr("data-readonly") !== "true" && $(this).attr("minlength")) {
                if ($(this).val() !== "" && $(this).val().length < $(this).attr("minlength")) {
                    $(this).addClass('input-validation-error');
                    $(this).next("span").remove();
                    $("<span style='color:red;'><small>Minimum " + (typeof $(this).attr("masklength") !== "undefined" && $(this).attr("masklength").length > 0 ? $(this).attr("minlength") - $(this).attr("masklength") : $(this).attr("minlength")) + " length required</small></span>").insertAfter($(this))
                    return false;
                }
                else {
                    $(this).removeClass('input-validation-error');
                    $(this).siblings("label").next("span").remove();

                }

            }
            if ($(this).attr("data-readonly") !== "true" && $(this).attr("maxlength")) {
                if ($(this).val() !== "" && $(this).val().length < $(this).attr("maxlength")) {
                    //$(this).addClass('input-validation-error');
                    //$(this).siblings("label").next("span").remove();
                    //$("<span style='color:red;'><small>Maximum " + $(this).attr("maxlength") + " length required</small></span>").insertAfter($(this).siblings("label"))

                    return false;
                }
            }
        }
    });
}
function ToggleButton() {

    $('.typechecking input[type="checkbox"]').each(function (row, item) {
        if ($(item).is(":checked")) {
            $(item).parent().parent().find(".inactivecheck").removeClass("active")
            $(item).parent().parent().find(".activecheck").addClass("active")
        }

        else {
            $(item).parent().parent().find(".activecheck").removeClass("active")
            $(item).parent().parent().find(".inactivecheck").addClass("active")
        }
    });


    $('.typechecking input[type="checkbox"]').change(function () {
        if (this.checked) {
            $(this).parent().siblings(".activecheck").addClass("active");
            $(this).parent().siblings(".inactivecheck").removeClass("active");
        }
        else {
            $(this).parent().siblings(".inactivecheck").addClass("active");
            $(this).parent().siblings(".activecheck").removeClass("active");
        }
    })


    $(".activecheck").click(function () {
        $(this).next().children().prop('checked', false).trigger('click');
        $(this).addClass("active");
        $(this).siblings('.inactivecheck').removeClass("active");
    })
    $(".inactivecheck").click(function () {
        $(this).prev().children().prop('checked', true).trigger('click')
        $(this).addClass("active");
        $(this).siblings('.activecheck').removeClass("active");
    })

}

function ShowPreLoader() {
    $(".main-loader").show();
    $(".main-loader").addClass("loaderBg");
}

function HidePreLoader() {
    $(".main-loader").hide("slow");
    $(".main-loader").removeClass("loaderBg");
}

$(".datepicker").change(function () {
    if ($(this).val() != '') {
        $(this).parent().parent().parent().addClass("input--filled")
    }
});
$("input[type='text'] , input[type='password'],input[type='email'], textarea").focusin(function () {

    if ($(this).attr("id") == "m_typeahead_3") {
        $(".error-span").remove();
        $(this).parent("span").removeClass('input-validation-error');

    }



    //    $(this).siblings("label").next("span").remove();
    //    $(this).removeClass("input-validation-error")
    //});
    //$("input[type='checkbox']").change(function () {
    //    if (this.checked) {
    //        $(this).parent().siblings(".activecheck").addClass("active");
    //        $(this).parent().siblings(".inactivecheck").removeClass("active");
    //    }
    //    else {
    //        $(this).parent().siblings(".inactivecheck").addClass("active");
    //        $(this).parent().siblings(".activecheck").removeClass("active");
    //    }
})


function TextBox_Animation() {
    // trim polyfill : https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/Trim
    if (!String.prototype.trim) {
        (function () {
            // Make sure we trim BOM and NBSP
            var rtrim = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;
            String.prototype.trim = function () {
                return this.replace(rtrim, '');
            };
        })();
    }

    [].slice.call(document.querySelectorAll('input.input__field,textarea.input__field, textarea')).forEach(function (inputEl) {
        // in case the input is already filled..
        if (inputEl.value.trim() !== '') {
            classie.add(inputEl.parentNode, 'input--filled');
        }

        // events:
        inputEl.addEventListener('focus', onInputFocus);
        inputEl.addEventListener('blur', onInputBlur);
    });

    function onInputFocus(ev) {
        classie.add(ev.target.parentNode, 'input--filled');
    }

    function onInputBlur(ev) {
        if (ev.target.value.trim() === '') {
            classie.remove(ev.target.parentNode, 'input--filled');
        }
    }
}

function TextBox_Animation_2(window) {
    'use strict';

    function classReg(className) {
        return new RegExp("(^|\\s+)" + className + "(\\s+|$)");
    }
    // classList support for class management
    // altho to be fair, the api sucks because it won't accept multiple classes at once
    var hasClass, addClass, removeClass;

    if ('classList' in document.documentElement) {
        hasClass = function (elem, c) {
            return elem.classList.contains(c);
        };
        addClass = function (elem, c) {
            elem.classList.add(c);
        };
        removeClass = function (elem, c) {
            elem.classList.remove(c);
        };
    }
    else {
        hasClass = function (elem, c) {
            return classReg(c).test(elem.className);
        };
        addClass = function (elem, c) {
            if (!hasClass(elem, c)) {
                elem.className = elem.className + ' ' + c;
            }
        };
        removeClass = function (elem, c) {
            elem.className = elem.className.replace(classReg(c), ' ');
        };
    }

    function toggleClass(elem, c) {
        var fn = hasClass(elem, c) ? removeClass : addClass;
        fn(elem, c);
    }
    var classie = {
        // full names
        hasClass: hasClass,
        addClass: addClass,
        removeClass: removeClass,
        toggleClass: toggleClass,
        // short names
        has: hasClass,
        add: addClass,
        remove: removeClass,
        toggle: toggleClass
    };
    // transport
    if (typeof define === 'function' && define.amd) {
        // AMD
        define(classie);
    } else {
        // browser global
        window.classie = classie;
    }
}

function NavigateAlert() {
    swal({ text: 'Please fill previous information to move forword' });
}

function validateData() {
    var result = true;

    var check = true;

    $("input[data-required='true']:visible").each(function () {

        if ($(this).attr("data-readonly") !== "true" && !$(this).is('[disabled]') && $(this).val() == '') {
            if ($(this).attr("id") == "m_typeahead_3") {
                $(this).addClass('input-validation-error');
                $(this).next("span#RequiredLable").remove();
                $("<span id='RequiredLable' style='color:red;'><small>required</small></span>").insertAfter($(this))
                result = false;
                check = false;
            }
            else {
                if (!$(this).hasClass("tt-hint")) {
                    $(this).addClass('input-validation-error');
                    $(this).next("span#RequiredLable").remove();
                    $("<span id='RequiredLable' style='color:red;'><small>required</small></span>").insertAfter($(this))

                    result = false;
                    check = false;
                }
            }


        } else {
            $(this).removeClass('input-validation-error');
            $(this).next("span#RequiredLable").remove();
        }

    });
    $("textarea[data-required='true']:visible").each(function () {

        if ($(this).attr("data-readonly") !== "true" && $(this).val() == '') {

            $(this).addClass('input-validation-error');
            $(this).next("span").remove();
            $("<span style='color:red;'><small>required</small></span>").insertAfter($(this))
            result = false;
            check = false;

        } else {
            $(this).removeClass('input-validation-error');
            $(this).next("span").remove();

        }

    });

    $("select[data-required='true']:visible").each(function () {

        $(this).next().siblings("span").remove();
        $(this).removeClass('input-validation-error');
        if ($(this).attr("data-readonly") !== "true" && $(this).val() == '') {

            if ($(this).attr("data-readonly") !== "true" && $(this).val() == '') {
                $(this).next().children().children().addClass('input-validation-error');
                $("<span style='text-align:left;color:red;'><small>required</small></span>").insertAfter($(this).next())
                result = false;
            } else {
                $(this).next().siblings("span").remove();
                $(this).next().children().children().removeClass('input-validation-error');
            }


        } else {
            $(this).next().siblings("span").remove();
            $(this).next().children().children().removeClass('input-validation-error');

            $(this).removeClass('input-validation-error');
        }

    });

    $("input[data-number='true']:visible").each(function () {
        var pattern = /^[0-9]+(\.[0-9]{1,2})?$/;

        if ($(this).attr("data-readonly") !== "true" && !$(this).is('[disabled]') && $(this).val() == '') {
            $(this).addClass('input-validation-error');
            $(this).next("span#RequiredLable").remove();
            $("<span id='RequiredLable' style='color:red;'><small>required</small></span>").insertAfter($(this))
            result = false;
        }
        else {
            $(this).removeClass('input-validation-error');
            $(this).next("span#RequiredLable").remove();

            if ($(this).attr("data-readonly") !== "true" && $(this).val() !== "" && !$.isNumeric($(this).val())) {

                $(this).addClass('input-validation-error');
                $(this).next("span").remove();
                $("<span style='color:red;'><small>Value is invalid</small></span>").insertAfter($(this))
                result = false;

            } else {

                $(this).removeClass('input-validation-error');
                $(this).next("span#RequiredLable").remove();
            }
        }


    });

    $("input[data-phoneno='true']:visible").each(function () {
        var val = $(this).val();
        //'/^[0-9 ]+$/'
        var re = /^([0-9\n]+)$/g;
        var re1 = /^([0-9\n]+)/g;
        if ($(this).attr("data-readonly") !== "true" && re.test(val)) {
            $(this).removeClass('input-validation-error');
        } else {
            val = re1.exec(val);
            if (!val) {
                $(this).addClass('input-validation-error');
                $(this).next("span").remove();
                $("<span style='color:red;'><small>Number required</small></span>").insertAfter($(this))
                result = false;

            }
        }

    });

    $("input[data-email='true']:visible").each(function () {
        var val = $(this).val();
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        var tes = regex.test(val);

        if ($(this).attr("data-readonly") !== "true" && val != "" && !tes) {
            $(this).addClass('input-validation-error');
            $(this).next("span").remove();
            $("<span style='color:red;'><small>Invalid email format</small></span>").insertAfter($(this))
            result = false;

        } else {
            //$(this).removeClass('input-validation-error');
        }

    });

    $("input[data-number='2']:visible").each(function () {
        var val = $(this).val();
        var re = /^((\d+)(\.\d{1,2})|(\.\d{1,2}))$/g;
        var re1 = /^((\d+)(\.\d{1,2})|(\.\d{1,2}))/g;

        if ($(this).attr("data-readonly") !== "true" && re.test(val)) {

            $(this).addClass('input-validation-error');
        } else {

            val = re1.exec(val);
            if (val) {
                result = false;

                $($(this)).addClass('input-validation-error');
            } else {
                if (isNaN(val) && val != '.') {
                    result = false;

                    $(this).addClass('input-validation-error');
                }
            }
        }

    });
    $("input[data-zip='true']:visible").each(function () {

        // $(this).parent().next("span").remove();
        var regPostalCode = /^([0-9]{5})(?:[-\s]*([0-9]{4}))?$/;
        var postal_code = $(this).val();
        if (regPostalCode.test(postal_code) == false) {
            //$(obj).removeAttr('data-required');
            $(this).addClass('input-validation-error');
            $(this).parent().next("span#RequiredLable").remove();
            //  $("<span style='color:red;'><small>Invalid Zip</small></span>").insertAfter($(this).parent())
            result = false;
            check = false;
        }
        else {
            $(this).removeClass('input-validation-error');
            //$(this).parent().next("span").remove();
        }

    });

    if (!check) {
        $("[minlength]:visible").each(function () {
            if ($(this).attr("data-readonly") !== "true" && $(this).val() != "" && $(this).val().length < $(this).attr("minlength")) {
                $(this).addClass('input-validation-error');
                $(this).siblings("label").next("span").remove();
                $("<span style='color:red;'><small>Minimum " + (typeof $(this).attr("masklength") !== "undefined" && $(this).attr("masklength").length > 0 ? $(this).attr("minlength") - $(this).attr("masklength") : $(this).attr("minlength")) + " length required</small></span>").insertAfter($(this).siblings("label"))

                result = false;

            } else {
                $(this).siblings("label").next("span").remove();
                $(this).removeClass("input-validation-error")
            }
        });
    }

    //focusOnValidation();
    return result;

}
//function ZipCheck(obj) {
//    //$(obj).parent().next("span").remove();
//    var regPostalCode = /^([0-9]{5})(?:[-\s]*([0-9]{4}))?$/;
//    var postal_code = $(obj).val();
//    if (regPostalCode.test(postal_code) == false) {
//        $(obj).removeAttr('data-required');
//        $(obj).addClass('input-validation-error');
//        //$("<span style='color:red;'><small>Invalid Zip</small></span>").insertAfter($(obj).parent())
//        //$(obj).val('');
//    }
//    else {
//        $(obj).removeClass('input-validation-error');
//        //$(obj).parent().next("span").remove();
//    }

//}

function NumericInput() {
    var result = true;
    //place a data-isNumericRestricted='true'
    //then via jquery selector apply keyup event here 

    $("input[data-isNumericRestricted='true']").each(function (index, a) {

        var val = a.value;
        var re = /^((\d+)(\.\d{0,2})?)$/g;
        var re1 = /^((\d+)(\.\d{0,2})?)/g;

        if (re.test(val)) {
            $(a).removeClass('input-validation-error');
        } else {
            //console.log(val2);
            if (re1.test(val)) {
                result = false;
                $(a).addClass('input-validation-error');
            } else {
                //console.log(isNaN(a.value) && a.value != '.');
                if (isNaN(val) && val != '.') {
                    result = false;

                    $(a).addClass('input-validation-error');
                }
                else {

                    $(a).addClass('input-validation-error');
                    result = false;
                }
            }
        }
    });
    return result;

}

function checkStrength2(password, result) {

    //initial strength
    var strength = 0

    if (password.length == 0) {
        result.removeClass()
        return ''
    }
    //if the password length is less than 7, return message.
    if (password.length < 12) {
        result.removeClass()
        result.addClass('normal')
        return 'Normal'
    }

    //length is ok, lets continue.

    //if length is 8 characters or more, increase strength value
    if (password.length > 12) strength += 1

    //if password contains both lower and uppercase characters, increase strength value
    if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1

    //if it has one special character, increase strength value
    if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1

    //if it has two special characters, increase strength value
    if (password.match(/"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{12,})"/)) strength += 1

    //now we have calculated strength value, we can return messages

    //if value is less than 2
    if (strength < 2) {
        result.removeClass()
        result.addClass('medium')
        return 'Medium'
    } else if (strength == 2) {
        result.removeClass()
        result.addClass('strong')
        return 'Strong'
    } else {
        result.removeClass()
        result.addClass('vstrong')
        return 'Very Strong'
    }
}
$('#divSearchResult img').bind('contextmenu', function (e) {
    return false;
});

$(document).ready(function () {
    // Validation for numers only
    $("input[data-number='true']").keyup(function () {
        //  var pattern = /^\d+$/;
        var pattern = /^d+(\.[0-9]{1,2})?$/;
        if ($(this).attr("data-readonly") !== "true" && !$.isNumeric($(this).val())) {
            //var searctb = $(this).val();
            //$(this).val(searctb.substring(0, searctb.length - 1));
            $(this).val('');

        } else {
            if ($(this).val().indexOf('-') > -1) {
                $(this).val('');
            }
            $(this).removeClass('input-validation-error');
        }
    })

    //------###### Validations #####-------//

    // Validation for alphabets only
    $("input[data-alpha='true']:visible").keyup(function () {
        var pattern = /\`|\~|\-|\_|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\""|\;|\:|\s/;
        if ($(this).attr("data-readonly") !== "true") {
            var newVal = $(this).val().replace(/\`|\~|\-|\_|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\""|\;|\:|\s/g, '')
            //newVal = newVal.replace(/[0-9]/g, "")
            $(this).val(newVal);

        } else {

            $(this).removeClass('input-validation-error');
        }
    })
    $("input[data-alpha='true']:visible").on('input propertychange paste', function () {
        var pattern = /\`|\~|\-|\_|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\""|\;|\:|[0-9]|\s/;
        if ($(this).attr("data-readonly") !== "true") {
            var newVal = $(this).val().replace(/\`|\~|\-|\_|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\""|\;|\:|[0-9]|\s/g, '')
            $(this).val(newVal);

        } else {

            $(this).removeClass('input-validation-error');
        }
    });


    // Validation for alphabets and hyphen("-") only
    $("input[data-alphahyphen='true']:visible").keyup(function () {
        var pattern = /\`|\~|\_|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\""|\;|\:|[0-9]|\s/;
        if ($(this).attr("data-readonly") !== "true") {
            var newVal = $(this).val().replace(/\`|\~|\_|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\""|\;|\:|[0-9]|\s/g, '')
            $(this).val(newVal);

        } else {

            $(this).removeClass('input-validation-error');
        }
    })
    $("input[data-alphahyphen='true']:visible").on('input propertychange paste', function () {
        var pattern = /\`|\~|\_|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\""|\;|\:|[0-9]|\s/;
        if ($(this).attr("data-readonly") !== "true") {
            var newVal = $(this).val().replace(/\`|\~|\_|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\""|\;|\:|[0-9]|\s/g, '')
            $(this).val(newVal);

        } else {

            $(this).removeClass('input-validation-error');
        }
    });

    // Validation for alphabets and numbers only
    $("input[data-alphanumber='true']:visible").keyup(function () {
        var pattern = /\`|\~|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\"|\;|\:/;
        if ($(this).attr("data-readonly") !== "true") {
            var newVal = $(this).val().replace(/\`|\~|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\"|\;|\:/g, '')
            $(this).val(newVal);

        } else {

            $(this).removeClass('input-validation-error');
        }
    })
    $("input[data-alphanumber='true']:visible").on('input propertychange paste', function () {
        var pattern = /\`|\~|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\"|\;|\:/;
        if ($(this).attr("data-readonly") !== "true") {
            var newVal = $(this).val().replace(/\`|\~|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\"|\;|\:/g, '')
            $(this).val(newVal);

        } else {

            $(this).removeClass('input-validation-error');
        }
    });



    $("select[data-required='true']:visible").change(function () {

        $(this).next().siblings("span").remove();
        if ($(this).attr("data-readonly") !== "true" && $(this).val() == '') {
            $(this).next().children().children().addClass('input-validation-error');
            $("<span style='color:red;'><small>required</small></span>").insertAfter($(this).next())
            result = false;

        } else {
            $(this).next().siblings("span").remove();
            $(this).next().children().children().removeClass('input-validation-error');
        }

    });
    $("[minlength]").keypress(function (e) {
        if (typeof $(this).attr("masklength") !== "undefined" && $(this).attr("masklength").length > 0) {
            if (e.which === 32)
                return false;
        }
    })

    $("[maxlength]").keyup(function () {
        if ($(this).attr("data-readonly") !== "true" && $(this).val() != "" && $(this).val().length > $(this).attr("maxlength")) {
            //$(this).addClass('input-validation-error');
            //  $(this).siblings("label").next("span").remove();
            //  $("<span style='color:red;'><small> Maximum " + $(this).attr("maxlength") + " length required</small></span>").insertAfter($(this).siblings("label"))
            return false;
        }
    })


    $('.alertdiv').hover(function () {
        //  $(this).find('.alertdivlisting').toggle();
    });


    //$(".activecheck").toggleClass("active")
    //$("input[type='checkbox']").change(function () {

    //    if (this.checked) {
    //        $(this).parent().siblings(".activecheck").addClass("active");
    //        $(this).parent().siblings(".inactivecheck").removeClass("active");
    //    }
    //    else {
    //        $(this).parent().siblings(".inactivecheck").addClass("active");
    //        $(this).parent().siblings(".activecheck").removeClass("active");
    //    }
    //})


    var result = $("#password-strength");

    $('#passwordn').keyup(function () {
        $(".bar-text").html(checkStrength($('#passwordn').val()))
    })

    function checkStrength(password) {

        //initial strength
        var strength = 0

        if (password.length == 0) {
            result.removeClass()
            return ''
        }
        //if the password length is less than 7, return message.
        if (password.length < 12) {
            result.removeClass()
            result.addClass('normal')
            return 'Normal'
        }

        //length is ok, lets continue.

        //if length is 8 characters or more, increase strength value
        if (password.length > 12) strength += 1

        //if password contains both lower and uppercase characters, increase strength value
        if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1

        //if it has one special character, increase strength value
        if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1

        //if it has two special characters, increase strength value
        if (password.match(/"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{12,})"/)) strength += 1

        //now we have calculated strength value, we can return messages

        //if value is less than 2
        if (strength < 2) {
            result.removeClass()
            result.addClass('medium')
            return 'Medium'
        } else if (strength == 2) {
            result.removeClass()
            result.addClass('strong')
            return 'Strong'
        } else {
            result.removeClass()
            result.addClass('vstrong')
            return 'Very Strong'
        }
    }
    $('#divSearchResult img').bind('contextmenu', function (e) {
        return false;
    });


});

function ShowAlert(msg, type) {
    var notifyEvent = {
        message: msg
    };
    $.notify(notifyEvent, {
        type: type,
        allow_dismiss: true,
        newest_on_top: true,
        mouse_over: true,
        showProgressbar: false,
        spacing: 10,
        timer: 3000,
        placement: {
            from: 'top',
            align: 'right'
        },
        offset: {
            x: 30,
            y: 30
        },
        delay: 1000,
        z_index: 10000,
        animate: {
            enter: "animated " + 'slideInDown',
            exit: "animated " + 'fadeOut'
        }
    });

}

$(".selectuser2").change(function () {
    if ($(this).val() != "0") {
        $(this).prev().addClass("showselectlbl")

    }
    else {
        $(this).prev().removeClass("showselectlbl")
    }


});




(function () {
    // trim polyfill : https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/Trim
    if (!String.prototype.trim) {
        (function () {
            // Make sure we trim BOM and NBSP
            var rtrim = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;
            String.prototype.trim = function () {
                return this.replace(rtrim, '');
            };
        })();
    }

    [].slice.call(document.querySelectorAll('input.input__field,textarea')).forEach(function (inputEl) {
        // in case the input is already filled..
        if (inputEl.value.trim() !== '') {
            classie.add(inputEl.parentNode, 'input--filled');
        }

        // events:
        inputEl.addEventListener('focus', onInputFocus);
        inputEl.addEventListener('blur', onInputBlur);
    });

    function onInputFocus(ev) {
        classie.add(ev.target.parentNode, 'input--filled');
    }

    function onInputBlur(ev) {
        if (ev.target.value.trim() === '') {
            classie.remove(ev.target.parentNode, 'input--filled');
        }
    }
})();

(function (window) {

    'use strict';

    // class helper functions from bonzo https://github.com/ded/bonzo

    function classReg(className) {
        return new RegExp("(^|\\s+)" + className + "(\\s+|$)");
    }

    // classList support for class management
    // altho to be fair, the api sucks because it won't accept multiple classes at once
    var hasClass, addClass, removeClass;

    if ('classList' in document.documentElement) {
        hasClass = function (elem, c) {
            return elem.classList.contains(c);
        };
        addClass = function (elem, c) {
            elem.classList.add(c);
        };
        removeClass = function (elem, c) {
            elem.classList.remove(c);
        };
    }
    else {
        hasClass = function (elem, c) {
            return classReg(c).test(elem.className);
        };
        addClass = function (elem, c) {
            if (!hasClass(elem, c)) {
                elem.className = elem.className + ' ' + c;
            }
        };
        removeClass = function (elem, c) {
            elem.className = elem.className.replace(classReg(c), ' ');
        };
    }

    function toggleClass(elem, c) {
        var fn = hasClass(elem, c) ? removeClass : addClass;
        fn(elem, c);
    }

    var classie = {
        // full names
        hasClass: hasClass,
        addClass: addClass,
        removeClass: removeClass,
        toggleClass: toggleClass,
        // short names
        has: hasClass,
        add: addClass,
        remove: removeClass,
        toggle: toggleClass
    };

    // transport
    if (typeof define === 'function' && define.amd) {
        // AMD
        define(classie);
    } else {
        // browser global
        window.classie = classie;
    }

})(window);


$('.rightselectedplan .planboxmain ').each(function () {
    if ($(this).html().trim().length == 0) {

        $(this).addClass("emptyplandiv");
    }
});

$(function () {

    $('.imgcontainer').on('dragover', function (e) {
        e.preventDefault();
        $(this).addClass('file-over');
        //$('svg path').show();
    });

    $('.imgcontainer').on('dragleave', function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).removeClass('file-over');
    });

    $('.imgcontainer').on('drop', function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).addClass('file-over').stop(true, true).css({
            background: '#fff'
        });
        $('.progress').toggleClass('complete');
        $('.image-holder').addClass('move');
    });



});



$(document).ready(function () {
    //$(".brokertick").click(function (e) {
    //    $(".addnewbrokermain").show();
    //    e.stopPropagation();
    //});

    //Menu Should remain open if its clicked from navigation             
    $('.mm-panels').find('a').not('.mm-fullsubopen').each(function (i, a) {

        $(this).click(function () {

            if ($(this).attr('href') === undefined) {

            } else {
                if ($(this).attr('href').indexOf('#') != -1) {



                } else {

                    try {
                        ClearFilter();
                    }
                    catch (err) {
                        // document.getElementById("demo").innerHTML = err.message;
                    }

                    localStorage["openmenu"] = "yes";
                }

            }

        });
    });
    var optionsMenu = localStorage["openmenu"];

    if (optionsMenu) {
        if (optionsMenu == "yes") {
            $('#my-icon').trigger('click');
            $("#SecondLogin").addClass("lgoutbtnblock");
            localStorage["openmenu"] = "no";


        }

    }


    $(document).click(function (e) {
        if (!$(e.target).is('.addnewbrokermain , .k-dropdown  , .k-list-scroller , #ddlBrokerCompany-list')) {
            //$(".addnewbrokermain").hide();
        }
    });
    NoTabAllowed();
    UserPrivSetter();
});
$("span[id*=multidocicon]").on("click", function () {
    $(".addnewbrokermain").hide();
    $(this).next().show();

})
// Data grid state manage
function saveGridState(gridId, list) {
    if (localStorage["grid-options"]) {
        localStorage.removeItem("grid-options-" + list);
    }
    localStorage["grid-options-" + list] = kendo.stringify($("#" + gridId).data("kendoGrid").getOptions());
}
function setGridState(gridId, list) {
    var options = localStorage["grid-options-" + list];
    if (options) {
        setTimeout(function () {
            $("#" + gridId).data("kendoGrid").setOptions(JSON.parse(options));
        }, 100)

        localStorage.removeItem("grid-options-" + list)
        return options;
    }
}



function NoTabAllowed() {

    $('.disableclass').prop('tabindex', '-1')
}




function focusOnValidation() {
    if ($(".input-validation-error").length > 0) {
        $('html, body').animate({
            scrollTop: $(".input-validation-error:first").offset().top - 70
        }, 1000);
        $('.input-validation-error:first').focus();
    }
}

function UserPrivSetter() {
    if ($('#WritePrivledges').val() == "Deny") {
        $('[data-priv="true"]').remove()
        // For Top 4 Products
        $("#ddlSortOrder").prop('disabled', true);

    }


    //EditProfile
    if ($('#InnerWritePrivs').val() == "EditProfile") {
        $("input").prop('readonly', true);

        $("#btnSaveUser").hide();
    }
    if ($('#InnerWritePrivs').val() == "Promo") {
        $("input").prop('readonly', true);
        // For Promo Add Page
        $("#StateId").prop('disabled', true);
        $("#PromoType").prop('disabled', true);
        $("#CommissionType").prop('disabled', true);
        var dropdownlist = $("#Brokerpromo").data("kendoDropDownList");
        dropdownlist.readonly();
        var multiSelect = $("#required").data("kendoMultiSelect");
        multiSelect.readonly();


    }
    if ($('#InnerWritePrivs').val() == "PromoDisabled") {
        $("input").prop('disabled', 'disabled');
        // For Promo Add Page
        $("#StateId").prop('disabled', true);
        $("#PromoType").prop('disabled', true);
        $("#CommissionType").prop('disabled', true);
        var dropdownlist = $("#Brokerpromo").data("kendoDropDownList");
        dropdownlist.readonly();
        var multiSelect = $("#required").data("kendoMultiSelect");
        multiSelect.readonly();


    }
    if ($('#InnerWritePrivs').val() == "BrokerCompanies") {
        $("input").prop('disabled', true);
    }
    if ($('#InnerWritePrivs').val() == "BrokerList") {
        $("input").prop('disabled', true);
    }
    if ($('#InnerWritePrivs').val() == "GeneralSettings") {
        $("input").prop('disabled', true);
        $("#dllkWhTDSP").prop('disabled', true);
        $("#dllkWhNonTDSP").prop('disabled', true);
        $("#dllkWhNonTDSP").prop('disabled', true);
        var EsgExpDate = $("#EsgExpDate").data("kendoDatePicker");
        EsgExpDate.readonly();
        var ExperianResiExpDate = $("#ExperianResiExpDate").data("kendoDatePicker");
        ExperianResiExpDate.readonly();
        var ExperianCommExpDate = $("#ExperianCommExpDate").data("kendoDatePicker");
        ExperianCommExpDate.readonly();

    }
    if ($('#InnerWritePrivs').val() == "Charges") {
        $("input").prop('disabled', true);
    }
    if ($('#InnerWritePrivs').val() == "Holidays") {
        $("input").prop('disabled', true);
        var StartDate = $("#StartDate").data("kendoDatePicker");
        StartDate.readonly();
        var ExpireDate = $("#ExpireDate").data("kendoDatePicker");
        ExpireDate.readonly();
    }
    if ($('#InnerWritePrivs').val() == "Users") {
        $("input").not('#accessaccordion input').prop('disabled', true);
        $("#RoleID").prop('disabled', true);

    }
    if ($('#InnerWritePrivs').val() == "Ercot") {
        $("input").prop('disabled', true);
        $("#StatusID").prop('disabled', true);
        $("#StatesID").prop('disabled', true);
        $("#PremiseType").prop('disabled', true);
        $("#POLRCustomerClassID").prop('disabled', true);
        $("#TDSPAMSIndicatior").prop('disabled', true);
        $("#TDSPID").prop('disabled', true);

    }
    if ($('#InnerWritePrivs').val() == "Top4Residential") {
        $("input").prop('disabled', true);


    }
    if ($('#InnerWritePrivs').val() == "Templates") {
        $("input").prop('disabled', true);
        $('#EmailContents').find('iframe').contents().find('body').attr('contenteditable', 'false')

    }

    if ($('#InnerWritePrivs').val() == "Product") {
        $("input").prop('readonly', true);
        $("#ProductDetails_EnrollmentTypeId").prop('readonly', true);
        $("#ProductDetails_UtilityId").prop('readonly', true);
        $("#ProductDetails_ProductTypeID").prop('readonly', true);
        $("#ProductDetails_Description").prop('readonly', true);
        $("#ProductDetails_ProductCategoryTypeId").prop('readonly', true);
        var EFLDate = $("#ProductDetails_EFLDate").data("kendoDatePicker");
        EFLDate.readonly();

    }
}

function ContextMenuStop(DivId) {
    $('#' + DivId + ' img').bind('contextmenu', function (e) {
        return false;
    });
}

$(window).bind("pageshow", function () {
    var form = $('form').filter('form[name="NoFilterList"]');
    if (form[0] != null) {

        form[0].reset();
    }


});

function ListingHeaderSet(DivId) {



    if ($(DivId).find('tbody').height() <= $(DivId).height()) {
        $(".k-grid-header").addClass("removepaddingtable")

    }
    else {

        $(".k-grid-header").removeClass("removepaddingtable")
    }



}
//Past Searches
function SaveSearch(keyword, entity) {
    if (keyword) {
        ShowPreLoader();
        $.ajax({
            type: "GET",
            url: '/Base/SaveSearch',
            data: { KeyWord: keyword, Entity: entity },
            success: function (data) {
                // ResetPastSearches()
                HidePreLoader();
            },
            error: function (xhr, status, error) {
                //  HidePreLoader();
                var err = eval("(" + xhr.responseText + ")");
            }
        });
        HidePreLoader();
    }
}
function GetSavedSearch(entity) {

    $.ajax({
        type: "GET",
        url: '/Base/GetSavedSearches',
        data: { Entity: entity },
        success: function (data) {
            if (data) {
                var searchList = "";
                for (var i = 0; i < data.length; i++) {
                    searchList += "<div class='searchrowmargin'  data-past='true'><a href='javascript:void(0)' onclick='filterPast(this)' title='" + data[i].Keyword + "'>" + data[i].Keyword + "</a></div>";
                }
            }
            $(searchList).insertAfter($("#past_search"));

        },
        error: function (xhr, status, error) {
            HidePreLoader();
            var err = eval("(" + xhr.responseText + ")");
        }
    });
}
//Form cancel
function cancel(url) {
    swal({
        title: "Are you sure?",
        text: "you want to cancel ?",
        type: "warning",
        allowEscapeKey: false,
        allowOutsideClick: false,
        showCancelButton: !0,
        cancelButtonText: "No",
        confirmButtonText: "Yes, cancel it!",
        confirmButtonClass: "btn btn-danger m-btn m-btn--custom"
    }).then(function (e) {
        if (e.dismiss != "cancel")
            location.href = url;
    })

}
function AjaxCall(url, dataToService, methodType, successCallBack, failureCallBack, errorCallBack, completeCallBack, loaderShowCalBack, loaderHideCallBack) {
    if (loaderShowCalBack !== null && typeof loaderShowCalBack !== 'undefined') {
        loaderShowCalBack.call();
    }
    jQuery.ajax({
        type: methodType,
        url: url,
        data: dataToService,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        processData: true,
        success: function (result) {
            if (successCallBack !== null && typeof successCallBack !== 'undefined') {
                successCallBack.call(undefined, result, dataToService);
            }

        },
        failure: function (xhr) {
            if (failureCallBack !== null && typeof failureCallBack !== 'undefined') {
                failureCallBack.call(undefined, xhr);
            }
        },
        error: function (xhr) {
            if (errorCallBack !== null && typeof errorCallBack !== 'undefined') {
                errorCallBack.call(undefined, xhr);
            }
        },
        complete: function (result) {
            if (completeCallBack !== null && typeof completeCallBack !== 'undefined') {
                completeCallBack.call(undefined, result);
            }
            if (loaderHideCallBack !== null && typeof loaderHideCallBack !== 'undefined') {
                loaderHideCallBack.call();
            }
        }
    });
}
var todayDate = function () {
    today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //As January is 0.
    var yyyy = today.getFullYear();
    var hh = today.getHours();
    var minut = today.getMinutes();
    var ss = today.getSeconds();

    if (dd < 10) dd = '0' + dd;
    if (mm < 10) mm = '0' + mm;
    return (yyyy + '-' + mm + '-' + dd + ' ' + hh + ':' + minut + ':' + ss);
};
//block ui
function showLoader() {
    mApp.block(".blockui-loader", {
        overlayColor: "#000000",
        type: "loader",
        state: "primary",
        message: ""
    });
}
function hideLoader() {
    mApp.unblock(".blockui-loader");
}
var yas;
function arabicValue(txt) {
    yas = txt.value;
    yas = yas.replace(/`/g, "ذ");
    yas = yas.replace(/0/g, "۰");
    yas = yas.replace(/1/g, "۱");
    yas = yas.replace(/2/g, "۲");
    yas = yas.replace(/3/g, "۳");
    yas = yas.replace(/4/g, "٤");
    yas = yas.replace(/5/g, "۵");
    yas = yas.replace(/6/g, "٦");
    yas = yas.replace(/7/g, "۷");
    yas = yas.replace(/8/g, "۸");
    yas = yas.replace(/9/g, "۹");
    yas = yas.replace(/0/g, "۰");
    yas = yas.replace(/q/g, "ض");
    yas = yas.replace(/w/g, "ص");
    yas = yas.replace(/e/g, "ث");
    yas = yas.replace(/r/g, "ق");
    yas = yas.replace(/t/g, "ف");
    yas = yas.replace(/y/g, "غ");
    yas = yas.replace(/u/g, "ع");
    yas = yas.replace(/i/g, "ه");
    yas = yas.replace(/o/g, "خ");
    yas = yas.replace(/p/g, "ح");
    yas = yas.replace(/\[/g, "ج");
    yas = yas.replace(/\]/g, "د");
    yas = yas.replace(/a/g, "ش");
    yas = yas.replace(/s/g, "س");
    yas = yas.replace(/d/g, "ي");
    yas = yas.replace(/f/g, "ب");
    yas = yas.replace(/g/g, "ل");
    yas = yas.replace(/h/g, "ا");
    yas = yas.replace(/j/g, "ت");
    yas = yas.replace(/k/g, "ن");
    yas = yas.replace(/l/g, "م");
    yas = yas.replace(/\;/g, "ك");
    yas = yas.replace(/\'/g, "ط");
    yas = yas.replace(/z/g, "ئ");
    yas = yas.replace(/x/g, "ء");
    yas = yas.replace(/c/g, "ؤ");
    yas = yas.replace(/v/g, "ر");
    yas = yas.replace(/b/g, "لا");
    yas = yas.replace(/n/g, "ى");
    yas = yas.replace(/m/g, "ة");
    yas = yas.replace(/\,/g, "و");
    yas = yas.replace(/\./g, "ز");
    yas = yas.replace(/\//g, "ظ");
    yas = yas.replace(/~/g, " ّ");
    yas = yas.replace(/Q/g, "َ");
    yas = yas.replace(/W/g, "ً");
    yas = yas.replace(/E/g, "ُ");
    yas = yas.replace(/R/g, "ٌ");
    yas = yas.replace(/T/g, "لإ");
    yas = yas.replace(/Y/g, "إ");
    yas = yas.replace(/U/g, "‘");
    yas = yas.replace(/I/g, "÷");
    yas = yas.replace(/O/g, "×");
    yas = yas.replace(/P/g, "؛");
    yas = yas.replace(/A/g, "ِ");
    yas = yas.replace(/S/g, "ٍ");
    yas = yas.replace(/G/g, "لأ");
    yas = yas.replace(/H/g, "أ");
    yas = yas.replace(/J/g, "ـ");
    yas = yas.replace(/K/g, "،");
    yas = yas.replace(/L/g, "/");
    yas = yas.replace(/Z/g, "~");
    yas = yas.replace(/X/g, "ْ");
    yas = yas.replace(/B/g, "لآ");
    yas = yas.replace(/N/g, "آ");
    yas = yas.replace(/M/g, "’");
    yas = yas.replace(/\?/g, "؟");
    txt.value = yas;
}
function formatPhoneNumber(phoneNumberString) {
    var cleaned = ('' + phoneNumberString).replace(/\D/g, '')
    var match = cleaned.match(/^(\d{3})(\d{3})(\d{4})$/)
    if (match) {
        return '(' + match[1] + ') ' + match[2] + '-' + match[3]
    }
    return null
}
function formatPhone(txt) {
    var farmattedNumber = '';
    if (txt != null && txt != '') {
        var numbers = txt.replace(/\D/g, ''),
            char = { 0: '+', 3: ' ', 7: ' ' };
        for (var i = 0; i < numbers.length; i++) {
            farmattedNumber += (char[i] || '') + numbers[i];
        }
    }
    return farmattedNumber;
}
function ViewImage(obj) {
    var viewer = ImageViewer();
    var imgSrc = obj.src,
        highResolutionImage = $(obj).data('data-high-res-src');
    viewer.show(imgSrc, highResolutionImage);
}
function time24Convertor(time) {
    var orignalTime = time
    var PM = time.match('PM') ? true : false

    time = time.split(':')
    var min = time[1]

    if (PM) {
        if (parseInt(time[0], 10) < 12) {
            var hour = 12 + parseInt(time[0], 10)
            //var sec = time[1].replace('PM', '')
            min = min.replace('PM', '').trim()
        } else {
            var hour = parseInt(time[0], 10)
            //var sec = time[1].replace('PM', '')
            min = min.replace('PM', '').trim()
        }

    } else {
        var hour = time[0]
        //var sec = time[1].replace('AM', '')
        min = min.replace('AM', '').trim()
    }

    return hour + ':' + min + ':00';
}
var getInitials = function (string) {
    var names = string.split(' '),
        initials = names[0].substring(0, 1).toUpperCase();

    if (names.length > 1) {
        initials += names[names.length - 1].substring(0, 1).toUpperCase();
    }
    return initials;
};
propSort = function (array, prop, desc) {
    array.sort(function (a, b) {
        if (a[prop] < b[prop])
            return desc ? 1 : -1;
        if (a[prop] > b[prop])
            return desc ? -1 : 1;
        return 0;
    });
}
var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return typeof sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
    return false;
};


function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}
function eraseCookie(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}


$('#changePasswordModal').click(function (e) {
    GetCurrentLoginInfo();
    if ($('#divAreaPasswordChange').css('display') == 'none') {
        $('#divAreaPasswordChange').show(500);
    }
    else {
        $('#divAreaPasswordChange').hide(500);
    }
})
$('#btnChangePassword').click(function (e) {
    //new_password
    //confirm_password

    var _newPassword = $('#new_password').val()
    var _confirmPassword = $('#confirm_password').val()
    var _fullname = $('#txtfullname').val()
    var _emailaddress = $('#txtEmailAddress').val()


    if (_newPassword != _confirmPassword) {
        swal.fire({
            title: "Oops!",
            text: "Password doesn't matched!",
            icon: "error",
            buttonsStyling: false,
            confirmButtonText: "Ok, got it!",
            customClass: {
                confirmButton: "btn font-weight-bold btn-light-primary"
            }
        }).then(function () {
            KTUtil.scrollTop();
        });
    }
    else if (_newPassword == '') {
        swal.fire({
            title: "Oops!",
            text: "Fill all inputs",
            icon: "error",
            buttonsStyling: false,
            confirmButtonText: "Ok, got it!",
            customClass: {
                confirmButton: "btn font-weight-bold btn-light-primary"
            }
        }).then(function () {
            KTUtil.scrollTop();
        });
    }
    else if (_confirmPassword == '') {
        swal.fire({
            title: "Oops!",
            text: "Fill all inputs",
            icon: "error",
            buttonsStyling: false,
            confirmButtonText: "Ok, got it!",
            customClass: {
                confirmButton: "btn font-weight-bold btn-light-primary"
            }
        }).then(function () {
            KTUtil.scrollTop();
        });
    }
    else if (_fullname == '') {
        swal.fire({
            title: "Oops!",
            text: "Fill all inputs",
            icon: "error",
            buttonsStyling: false,
            confirmButtonText: "Ok, got it!",
            customClass: {
                confirmButton: "btn font-weight-bold btn-light-primary"
            }
        }).then(function () {
            KTUtil.scrollTop();
        });
    }
    else if (_emailaddress == '') {
        swal.fire({
            title: "Oops!",
            text: "Fill all inputs",
            icon: "error",
            buttonsStyling: false,
            confirmButtonText: "Ok, got it!",
            customClass: {
                confirmButton: "btn font-weight-bold btn-light-primary"
            }
        }).then(function () {
            KTUtil.scrollTop();
        });
    }
    else {
        e.preventDefault();
        var params = {
            id: $('#txtLoginUserId').val(),
            new_password: _confirmPassword,
            user_role: $('#txtUserRole').val(),
            fullname: $('#txtfullname').val(),
            email_address: $('#txtEmailAddress').val()
        }
        AjaxCall('/Home/ChangeSettings', JSON.stringify(params), 'POST', onsuccess);
        function onsuccess(response) {
            if (response.success === true) {
                swal.fire({
                    title: "Done!",
                    text: response.message,
                    icon: "success",
                    buttonsStyling: false,
                    confirmButtonText: "Ok, got it!",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    KTUtil.scrollTop();
                });
                $('#divAreaPasswordChange').hide(500);
                GetCurrentLoginInfo();
            } else {
                swal.fire({
                    text: response.message,
                    icon: "error",
                    buttonsStyling: false,
                    confirmButtonText: "Ok, got it!",
                    customClass: {
                        confirmButton: "btn font-weight-bold btn-light-primary"
                    }
                }).then(function () {
                    KTUtil.scrollTop();
                });
            }
        }
    }
})

function GetCurrentLoginInfo() {
    AjaxCall('/Home/GetUserSettings', null, 'GET', onsuccess);
    function onsuccess(response) {
        $('#txtfullname').val(response.fullname)
        $('#txtEmailAddress').val(response.emailaddress)
    }
}