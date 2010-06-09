package com.sihus.core.util;

import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.Rectangle;
import java.awt.RenderingHints;
import java.awt.geom.AffineTransform;
import java.awt.image.BufferedImage;
import java.awt.image.ColorModel;
import java.awt.image.WritableRaster;
import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;

import javax.imageio.ImageIO;

import com.sun.image.codec.jpeg.JPEGCodec;
import com.sun.image.codec.jpeg.JPEGEncodeParam;
import com.sun.image.codec.jpeg.JPEGImageEncoder;

/**
 * 图片图像实用类。<br/>
 * 
 * @author huangyongqiang
 * @since 2010-05-22
 */
public final class ImageUtil {

	private static final CommonLog COMMON_LOG = CommonLog.getLog(ImageUtil.class);

	/**
	 * 实现图像的等比缩放
	 * 
	 * @param source
	 *            原图片数据
	 * @param targetWidth
	 *            缩放目标宽度
	 * @param targetHeight
	 *            缩放目标高度
	 * @return
	 */
	public static BufferedImage resize(BufferedImage source, int targetWidth,
			int targetHeight) {
		BufferedImage ret = null;
		if (source != null && source.getWidth() > 0 && source.getHeight() > 0
				&& targetWidth > 0 && targetHeight > 0) {
			int type = source.getType();

			double sx = (double) targetWidth / source.getWidth();
			double sy = (double) targetHeight / source.getHeight();
			// 这里想实现在targetW，targetH范围内实现等比缩放。如果不需要等比缩放
			// 则将下面的if else语句注释即可
			if (sx < sy) {
				sx = sy;
				targetWidth = (int) (sx * source.getWidth());
			} else {
				sy = sx;
				targetHeight = (int) (sy * source.getHeight());
			}
			if (type == BufferedImage.TYPE_CUSTOM) { // handmade
				ColorModel cm = source.getColorModel();
				WritableRaster raster = cm.createCompatibleWritableRaster(
						targetWidth, targetHeight);
				boolean alphaPremultiplied = cm.isAlphaPremultiplied();
				ret = new BufferedImage(cm, raster, alphaPremultiplied, null);
			} else {
				ret = new BufferedImage(targetWidth, targetHeight, type);
			}
			Graphics2D g = ret.createGraphics();
			// smoother than exlax:
			g.setRenderingHint(RenderingHints.KEY_INTERPOLATION,
					RenderingHints.VALUE_INTERPOLATION_BICUBIC);
			g.drawRenderedImage(source, AffineTransform
					.getScaleInstance(sx, sy));
			g.dispose();
		}
		return ret;
	}

	/**
	 * 实现图像的等比缩放和缩放后的截取
	 * 
	 * @param inFilePath
	 *            要截取文件的路径
	 * @param outFilePath
	 *            截取后输出的路径
	 * @param width
	 *            要截取宽度
	 * @param hight
	 *            要截取的高度
	 * @param proportion
	 * @throws Exception
	 */
	public static synchronized void saveImageAsJpg(String inFilePath, String outFilePath,
			int width, int hight, boolean proportion) throws Exception {
		if (StringUtils.isNull(inFilePath) || StringUtils.isNull(outFilePath)) {
			return;
		}
		File file = new File(inFilePath);
		if(!file.exists() ||! file.isFile()) {
			return;
		}
		File saveFile = new File(outFilePath);
		if(!saveFile.isFile()) {
			return;
		}
		saveFile.deleteOnExit();
		InputStream in = new FileInputStream(file);
		BufferedImage srcImage = ImageIO.read(in);
		if (width > 0 || hight > 0) {
			// 原图的大小
			int sw = srcImage.getWidth();
			int sh = srcImage.getHeight();
			// 如果原图像的大小小于要缩放的图像大小，直接将要缩放的图像复制过去
			if (sw > width && sh > hight) {
				srcImage = resize(srcImage, width, hight);
			} else {
				String fileName = saveFile.getName();
				String formatName = fileName.substring(fileName
						.lastIndexOf('.') + 1);
				ImageIO.write(srcImage, formatName, saveFile);
				return;
			}
		}
		// 缩放后的图像的宽和高
		int w = srcImage.getWidth();
		int h = srcImage.getHeight();
		// 如果缩放后的图像和要求的图像宽度一样，就对缩放的图像的高度进行截取
		if (w == width) {
			// 计算X轴坐标
			int x = 0;
			int y = h / 2 - hight / 2;
			writeImage(srcImage, new Rectangle(x, y, width, hight),
					saveFile);
		} else if (h == hight) {
			// 否则如果是缩放后的图像的高度和要求的图像高度一样，就对缩放后的图像的宽度进行截取
			// 计算X轴坐标
			int x = w / 2 - width / 2;
			int y = 0;
			writeImage(srcImage, new Rectangle(x, y, width, hight),
					saveFile);
		}
		in.close();
	}

	/**
	 * 存储缩放后的截图
	 * 
	 * @param image
	 *            缩放后的图像
	 * @param subImageBounds
	 *            要截取的子图的范围
	 * @param subImageFile
	 *            要保存的文件
	 * @throws IOException
	 */
	public static  synchronized void writeImage(BufferedImage image,
			Rectangle subImageBounds, File subImageFile) throws IOException {
		if (null == image) {
			COMMON_LOG.warn("Bad image");
			return;
		}
		if (null == subImageBounds) {
			String fileName = subImageFile.getName();
			String formatName = fileName
					.substring(fileName.lastIndexOf('.') + 1);
			ImageIO.write(image, formatName, subImageFile);
		} else {
			if (subImageBounds.x < 0
					|| subImageBounds.y < 0
					|| subImageBounds.width - subImageBounds.x > image
							.getWidth()
					|| subImageBounds.height - subImageBounds.y > image
							.getHeight()) {
				COMMON_LOG.warn("Bad subimage bounds");
				return;
			}
			BufferedImage subImage = image.getSubimage(subImageBounds.x,
					subImageBounds.y, subImageBounds.width,
					subImageBounds.height);
			String fileName = subImageFile.getName();
			String formatName = fileName
					.substring(fileName.lastIndexOf('.') + 1);
			ImageIO.write(subImage, formatName, subImageFile);
		}
	}

	/**
	 * 取得图片等比缩放后的宽高
	 * 
	 * @param srcWidth
	 *            原图片宽度
	 * @param srcHeight
	 *            原图片高度
	 * @param destWidth
	 *            目标图片宽度
	 * @param destHeight
	 *            目标图片宽度
	 * @return 数组，第一元素为宽度，第二个元素为高度
	 */
	public static int[] getWidthAndHeight(int srcWidth, int srcHeight,
			int destWidth, int destHeight) {
		int[] wh = new int[2];
		int w = destWidth;
		int h = destHeight;
		int sw = srcWidth;
		int sh = srcHeight;
		if (w > 0 && h > 0) {
			double sScale = (double) sw / sh;
			double nScale = (double) w / h;
			if (sScale >= nScale && sw > w) {
				sw = w;
				sh = (int) (sw / sScale);
			} else if (sh > h) {
				sh = h;
				sw = (int) (sh * sScale);
			}
		}
		wh[0] = sw;
		wh[1] = sh;
		return wh;
	}

	/**
	 * 强制按照宽高缩放图片
	 * 
	 * @param targetWidth
	 *            生成图片目标宽度
	 * @param targetHeight
	 *            生成图片目标高度
	 * @param filePath
	 *            生成图片文件路径
	 * @param ins
	 *            原始图片数据
	 * @return
	 */
	public static synchronized boolean writeFixImage(int targetWidth, int targetHeight,
			String filePath, InputStream ins) {
		boolean ret = false;
		OutputStream out = null;
		if(StringUtils.isNull(filePath)) {
			COMMON_LOG.warn("无效的文件路径");
			return false;
		}
		if(null==ins) {
			COMMON_LOG.warn("无效的输入流");
			return false;
		}
		try {
			File f = new File(filePath);
			f.deleteOnExit();
			if (!f.getParentFile().exists()) {
				f.getParentFile().mkdirs();
			}
			out = new FileOutputStream(filePath); // 输出到文件流
			ret = write(targetWidth, targetHeight, out, ins,
					BufferedImage.TYPE_INT_RGB, false);
		} catch (Exception e) {
			ret = false;
			COMMON_LOG.error("输出图片出错", e);
		}
		return ret;
	}

	/**
	 * 
	 * 按照宽高缩放图片
	 * 
	 * @param targetWidth
	 *            生成图片目标宽度
	 * @param targetHeight
	 *            生成图片目标高度
	 * @param filePath
	 *            生成图片文件路径
	 * @param ins
	 *            原始图片数据
	 * @param out
	 *            目标文件输出流
	 * @param type
	 *            生成的图片类型
	 * @param isFix
	 *            是否强制生成
	 * @return
	 */
	public static boolean write(int w, int h, OutputStream out,
			InputStream ins, int type, boolean isFix) {
		boolean ret = false;

		try {
			// 构造Image对象
			BufferedImage src = ImageIO.read(ins);
			int sw = src.getWidth(null);
			int sh = src.getHeight(null);
			if (isFix) {
				int[] wh = getWidthAndHeight(sw, sh, w, h);
				w = wh[0];
				h = wh[1];
			}
			w = w > 0 ? w : sw;
			h = h > 0 ? h : sh;
			// 得到缩小后的
			Image scaled = src.getScaledInstance(w, h, Image.SCALE_SMOOTH);
			// 绘制缩小/放大后的图
			BufferedImage thumbnail = new BufferedImage(w, h, type);
			thumbnail.getGraphics().drawImage(scaled, 0, 0, null);
			// 这里依赖了Sun的包，要不要使用第三方的包
			JPEGImageEncoder encoder = JPEGCodec.createJPEGEncoder(out);
			JPEGEncodeParam param = encoder
					.getDefaultJPEGEncodeParam(thumbnail);
			param.setQuality(1.0f, false);
			// JPEG编码
			encoder.encode(thumbnail);
			ret = true;
		} catch (IOException e) {
			ret = false;
			COMMON_LOG.error("输出图片出错", e);
		} finally {
			try {
				if (ins != null) {
					ins.close();
				}
				if (out != null) {
					out.close();
				}
			} catch (IOException e) {
				COMMON_LOG.error("关闭输入输出流出错", e);
			}
		}
		return ret;
	}

	/**
	 * 取得图片原始大小
	 * 
	 * @param imageData
	 *            图片数据
	 * @return 数组，第一元素为宽度，第二个元素为高度
	 */
	public static Integer[] getImageOriginalSize(byte[] imageData) {
		Integer[] size = null;
		if (imageData != null && imageData.length > 0) {
			size = getImageOriginalSize(new ByteArrayInputStream(imageData));
		}
		return size;
	}

	/**
	 * 取得图片原始大小
	 * 
	 * @param imageData
	 *            图片数据
	 * @return 数组，第一元素为宽度，第二个元素为高度
	 */
	public static Integer[] getImageOriginalSize(InputStream in) {
		Integer[] size = null;
		if (in != null) {
			try {
				BufferedImage sourceImg = ImageIO.read(in);
				int width = sourceImg.getWidth();
				int height = sourceImg.getHeight();
				size = new Integer[2];
				size[0] = width;
				size[1] = height;
			} catch (IOException e) {
				COMMON_LOG.error("读取出图片出错", e);
			}
		}
		return size;
	}
}
