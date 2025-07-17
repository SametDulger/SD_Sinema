// Write your JavaScript code.
$(document).ready(function() {
    // Form validation enhancement
    $('form').on('submit', function() {
        var isValid = true;
        
        // Check required fields
        $(this).find('[required]').each(function() {
            if (!$(this).val()) {
                $(this).addClass('is-invalid');
                isValid = false;
            } else {
                $(this).removeClass('is-invalid');
            }
        });
        
        // Email validation
        $(this).find('input[type="email"]').each(function() {
            var email = $(this).val();
            var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (email && !emailRegex.test(email)) {
                $(this).addClass('is-invalid');
                isValid = false;
            }
        });
        
        return isValid;
    });
    
    // Remove validation classes on input
    $('input, select, textarea').on('input change', function() {
        $(this).removeClass('is-invalid');
    });
    
    // Auto-hide alerts after 5 seconds
    setTimeout(function() {
        $('.alert').fadeOut('slow');
    }, 5000);
    
    // Confirm delete actions
    $('.btn-danger').on('click', function(e) {
        if (!confirm('Bu işlemi gerçekleştirmek istediğinizden emin misiniz?')) {
            e.preventDefault();
        }
    });
    
    // Dynamic price calculation for tickets
    $('#TicketTypeId').on('change', function() {
        var ticketTypeId = $(this).val();
        if (ticketTypeId) {
            // You can add AJAX call here to get price from API
            // For now, we'll just show a placeholder
            $('#Price').val('');
        }
    });
    
    // Dynamic seat loading for sessions
    $('#SessionId').on('change', function() {
        var sessionId = $(this).val();
        if (sessionId) {
            // You can add AJAX call here to load available seats
            $('#SeatId').html('<option value="">Koltuk Seçin</option>');
        }
    });
    
    // Salon capacity check
    $('#SalonId').on('change', function() {
        var salonId = $(this).val();
        if (salonId) {
            // You can add AJAX call here to get salon capacity
            // and update seat number validation
        }
    });
    
    // Date picker enhancement
    $('input[type="date"]').on('change', function() {
        var selectedDate = new Date($(this).val());
        var today = new Date();
        today.setHours(0,0,0,0);
        var inputId = $(this).attr('id');
        if (inputId === 'BirthDate') {
            // Doğum tarihi için: gelecekteki tarih engellenir
            if (selectedDate > today) {
                alert('Doğum günü gelecekte olamaz.');
                $(this).val('');
            }
        } else {
            // Diğer date input'lar için: geçmiş tarih engellenir
            if (selectedDate < today) {
                alert('Geçmiş bir tarih seçemezsiniz.');
                $(this).val('');
            }
        }
    });
    
    // Number input validation
    $('input[type="number"]').on('input', function() {
        var value = $(this).val();
        var min = $(this).attr('min');
        var max = $(this).attr('max');
        
        if (min && value < min) {
            $(this).val(min);
        }
        if (max && value > max) {
            $(this).val(max);
        }
    });
    
    // Search functionality
    $('#searchInput').on('keyup', function() {
        var value = $(this).val().toLowerCase();
        $('.card').filter(function() {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
    
    // Sort functionality
    $('.sort-btn').on('click', function() {
        var column = $(this).data('column');
        var order = $(this).data('order') === 'asc' ? 'desc' : 'asc';
        
        // You can add AJAX call here to sort data
        $(this).data('order', order);
        $(this).find('i').toggleClass('fa-sort-up fa-sort-down');
    });
    
    // Pagination enhancement
    $('.pagination .page-link').on('click', function(e) {
        e.preventDefault();
        var page = $(this).data('page');
        // You can add AJAX call here to load page data
    });
    
    // Modal enhancement
    $('.modal').on('show.bs.modal', function() {
        // Reset form when modal opens
        $(this).find('form')[0].reset();
        $(this).find('.is-invalid').removeClass('is-invalid');
    });
    
    // Tooltip initialization
    $('[data-toggle="tooltip"]').tooltip();
    
    // Popover initialization
    $('[data-toggle="popover"]').popover();
    
    // Loading spinner
    $('form').on('submit', function() {
        if ($(this).find('.btn-primary').length) {
            $(this).find('.btn-primary').html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Yükleniyor...');
        }
    });
    
    // Success message auto-hide
    if ($('.alert-success').length) {
        setTimeout(function() {
            $('.alert-success').fadeOut('slow');
        }, 3000);
    }
    
    // Error message auto-hide
    if ($('.alert-danger').length) {
        setTimeout(function() {
            $('.alert-danger').fadeOut('slow');
        }, 5000);
    }
    
    // Table row hover effect
    $('.table tbody tr').hover(
        function() {
            $(this).addClass('table-hover');
        },
        function() {
            $(this).removeClass('table-hover');
        }
    );
    
    // Card hover effect enhancement
    $('.card').hover(
        function() {
            $(this).addClass('shadow-lg');
        },
        function() {
            $(this).removeClass('shadow-lg');
        }
    );
    
    // Form field focus effect
    $('.form-control').focus(function() {
        $(this).parent().addClass('focused');
    }).blur(function() {
        $(this).parent().removeClass('focused');
    });
    
    // Responsive table
    $('.table-responsive').each(function() {
        if ($(this).width() < 768) {
            $(this).addClass('table-sm');
        }
    });
    
    // Print functionality
    $('.print-btn').on('click', function() {
        window.print();
    });
    
    // Export functionality
    $('.export-btn').on('click', function() {
        var format = $(this).data('format');
        // You can add export functionality here
        alert('Export ' + format + ' functionality will be implemented.');
    });
}); 