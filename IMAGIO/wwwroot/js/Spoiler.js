document.addEventListener('DOMContentLoaded', function () {
    $(document).ready(function () {
        $('.edit_tool_title').click(function (event) {
            if ($('.edit_tools_frame').hasClass('one')) {
                $('.edit_tool_title').not($(this)).removeClass('active');
                $('.edit_tool_content').not($(this).next()).slideUp(300);
            }
            $(this).toggleClass('active').next().slideToggle(400);
        });
    });
});