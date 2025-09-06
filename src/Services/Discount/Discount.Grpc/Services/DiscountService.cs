using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbcontext): DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupan = request.Coupon.Adapt<Coupan>();
            if (coupan is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
            }
            dbcontext.Coupans.Add(coupan);
            await dbcontext.SaveChangesAsync();
            var coupanModel = coupan.Adapt<CouponModel>();
            return coupanModel;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupans.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found"));
            }
            dbcontext.Coupans.Remove(coupon);
            await dbcontext.SaveChangesAsync();
            return new DeleteDiscountResponse { Success = true };
        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupans.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon == null)
                coupon=new Coupan
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount Desc"
                };

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupan = request.Coupon.Adapt<Coupan>();
            if (coupan is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
            }
            dbcontext.Coupans.Update(coupan);
            await dbcontext.SaveChangesAsync();
            var coupanModel = coupan.Adapt<CouponModel>();
            return coupanModel;
        }
    }
}
