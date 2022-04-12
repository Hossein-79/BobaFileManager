// MODAL
function showModal(modal) {
    modal.addClass('active');
    $('body').addClass('modal-active');
}
function hideModal(modal) {
    modal.removeClass('active');
    $('body').removeClass('modal-active');
}
$(document).on('click', '[data-modal]', function() {
    var modalId = $(this).attr('data-modal');
    showModal($('#' + modalId));
});
$(document).on('click', '.modal-close', function() {
    hideModal($(this).closest('.modal'));
});
// HIDE MODAL ON CLICK OUTSIDE
$(document).on('click', '.modal', function(e) {
    if (e.target !== this) {
        return;
    }
    hideModal($(this));
});