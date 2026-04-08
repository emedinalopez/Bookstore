using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Books.Queries
{
    public class GetAllBooksQuery : IRequest<IEnumerable<BookDTO>>
    {
    }
}
