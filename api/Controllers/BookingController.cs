using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Booking;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace api.Controllers {
    [Route("api/Booking")]
    [ApiController]
    public class BookingController : ControllerBase {
        private readonly ApplicationDBContext _context;
        private readonly IBookingRepository _bookingRepo;
        private readonly IRoomRepository _roomRepo;


        public BookingController(ApplicationDBContext context, IBookingRepository bookingRepo, IRoomRepository roomRepo) {
            _context = context;
            _bookingRepo = bookingRepo;
            _roomRepo = roomRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoom() {
            var bookings = await _bookingRepo.GetAllAsync();
            var bookingDto = bookings.Select(b => b.ToRBookingDto());
            return Ok(bookingDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdBooking([FromRoute] int id) {

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = await _bookingRepo.GetByIdAsync(id);
            if(booking == null) {
                return NotFound();
            }
            return Ok(booking.ToRBookingDto());
        }


        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDtos bookingDto) {
            var bookingModel = bookingDto.CreateBookingDto();
            await _bookingRepo.CreateAsync(bookingModel);
            return CreatedAtAction(nameof(GetByIdBooking), new {id = bookingModel.Id}, bookingModel.ToRBookingDto());
        }


        [HttpDelete("{id:int}")]
            public async Task<IActionResult> DeleteCustomer([FromRoute] int id) {
                
                if(!ModelState.IsValid)
                return BadRequest(ModelState);

                var bookingDelete = await _bookingRepo.DeleteAsync(id);
                if (bookingDelete == null) {
                    return NotFound();
                }
                return NoContent();
        }




    }
}